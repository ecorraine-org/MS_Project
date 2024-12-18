using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// エネミー用コントローラー
/// </summary>
public class EnemyController : WorldObjectController
{
    [HideInInspector, Tooltip("エネミーステータスマネージャー")]
    EnemyStatusHandler enemyStatus;

    [HideInInspector, Tooltip("ステートマネージャー")]
    EnemyStateHandler enemyState;

    [HideInInspector, Tooltip("アニメーションマネージャー")]
    EnemyAnimManager animManager;

    [HideInInspector, Tooltip("スキルマネージャー")]
    EnemySkillManager skillManager;

    [HideInInspector, Tooltip("UIマネージャー")]
    EnemyUIManager uIManager;

    [HideInInspector, Tooltip("ダッシュハンドラー")]
    DashHandler dashHandler;

    [SerializeField, Header("ラストヒットできるかどうか？")]
    protected bool isKillable = false;

    [HideInInspector, Tooltip("エネミー毎行動")]
    private EnemyAction enemyAction;

    private Animator animator;
    private CapsuleCollider capsuleCollider;

    [HideInInspector, Tooltip("スポナー")]
    private EnemySpawner enemySpawner;

    [HideInInspector, Tooltip("ガーベージコレクター")]
    private ObjectCollector objectCollector;




    //-------------------------------------
    public AudioClip hitSE; // 再生する効果音
    private AudioSource audioSource;

    // ピッチ操作
    public float minPitch = 0.8f; // 最小値
    public float maxPitch = 1.2f; // 最大値
    //-------------------------------------

    public override void Awake()
    {
        base.Awake();

        if (!this.transform.GetChild(0).gameObject.TryGetComponent<EnemyStatusHandler>(out enemyStatus))
            CustomLogger.LogWarning(Status.GetType(), Status.name);
        enemyStatus.Init(this);

        BattleManager = BattleManager.Instance;

        capsuleCollider = this.GetComponent<CapsuleCollider>();

        objectCollector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject.GetComponent<ObjectCollector>();
    }

    public void Start()
    {
        gameObj = Instantiate(Resources.Load<GameObject>(Status.StatusData.gameObjPrefab), this.transform);

        rigidBody.mass = Status.StatusData.mass;
        type = Status.StatusData.ObjectType;

        animator = gameObj.GetComponentInChildren<Animator>();

        animManager = GetComponentInChildren<EnemyAnimManager>();
        animManager.Init(this);

        skillManager = GetComponentInChildren<EnemySkillManager>();
        skillManager.Init(this);

        EffectHandler = GetComponentInChildren<EffectHandler>();
        EffectHandler.Init(this);

        dashHandler = GetComponentInChildren<DashHandler>();

        uIManager = GetComponentInChildren<EnemyUIManager>();

        AttackCollider = GetComponentInChildren<AttackColliderManagerV2>();

        CapsuleCollider collider = gameObj.GetComponent<CapsuleCollider>();
        capsuleCollider.center = collider.center;
        capsuleCollider.height = collider.height;
        capsuleCollider.radius = collider.radius;

        enemyState = GetComponentInChildren<EnemyStateHandler>();

        enemyAction = gameObj.GetComponentInChildren<EnemyAction>();
       //enemyAction.Init(this);

        //全部初期化 test
        var enemyActions = gameObj.GetComponentsInChildren<EnemyAction>();

        foreach (var action in enemyActions)
        {
            action.Init(this);
        }

        enemyState.Init(this);
        if (enemyState == null) Debug.Log("enemyState NULL");

        allowAttack = true;

        enemySpawner = ParentSpawner.GetComponent<EnemySpawner>();
        if (enemySpawner == null) Debug.Log("enemySpawner NULL");

        // AudioSourceコンポーネントを取得--------
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSourceがない");
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Pitchをランダムに設定
        float randomPitch = Random.Range(minPitch, maxPitch);
        audioSource.pitch = randomPitch;

        Debug.Log($"AudioSourceのPitchをランダムに設定しました: {randomPitch}");
        //----------------------------------------
    }

    private void FixedUpdate()
    {
       // Move();

        Vector3 gravity = Physics.gravity * (rigidBody.mass * rigidBody.mass);
        rigidBody.AddForce(gravity * Time.deltaTime);
    }

    private void Update()
    {
        if (player == null) return;

        //フィニッシュ
        if (Status.CurrentHealth <= Status.StatusData.maxHealth / 2)
        {
            isKillable = true;
        }

        if (enemySpawner.enemyOnomatoPool.Count < enemySpawner.maxEnemyOnomatopoeiaCount)
        {
            canGenerateOnomatopoeia = true;
        }
        else
        {
            canGenerateOnomatopoeia = false;
        }

        /*
        Chase();

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= EnemyStatus.StatusData.chaseDistance)
        {
            Vector3 direction = player.position - transform.position;
            //進む方向に向く
            Quaternion newRotation = Quaternion.LookRotation(direction.normalized);
            newRotation.x = 0f;
            transform.rotation = newRotation;

            if (distance <= EnemyStatus.StatusData.attackDistance)
            {
                OnMovementInput?.Invoke(Vector3.zero);

                //攻撃
                OnAttack?.Invoke();
            }
            else
            {
                IsAttacking = false;
                //追跡
                OnMovementInput?.Invoke(direction.normalized);
            }
        }
        else
        {
            IsAttacking = false;
            //停止
            OnMovementInput?.Invoke(Vector3.zero);
            State.TransitionState(ObjectStateType.Idle);
        }

         OnDamaged?.Invoke();
        */
    }


    public void Move()
    {
        /*
        State.TransitionState(ObjectStateType.Walk);
        */
        if (MovementInput.magnitude > 0.1f && Status.MoveSpeed > 0)
        {
            RigidBody.velocity = MovementInput * Status.MoveSpeed;
        }
    }

    public void Chase()
    {
        if (enemyAction)
            enemyAction.Chase();
    }

    /// <summary>
    /// 攻撃に遷移する際に呼び出す
    /// </summary>
    public void StartAttackCoroutine()
    {
        StartCoroutine(nameof(AttackCoroutine));
    }

    public IEnumerator AttackCoroutine()
    {
        AllowAttack = false;
        yield return new WaitForSeconds(Status.ActionCooldown);

        AllowAttack = true;
    }

    public override void Hit(bool _canOneHitKill)
    {
        //被撃状態へ遷移
        isDamaged = true;

        HitReaction hitReaction = BattleManager.GetPlayerHitReaction();
        //ヒットストップ
        BattleManager.StartHitStop(animator);

        //エフェクト生成
       // EffectHandler.InstantiateSpriteHit();

        // ヒット時に音を鳴らす----------
        base.Hit(_canOneHitKill); // 親クラスの処理を呼び出し

        // 効果音を再生
        if (audioSource != null && hitSE != null)
        {
            audioSource.PlayOneShot(hitSE);

            // 再生する効果音の名前をデバッグログに表示
            Debug.Log($"再生された効果音: {hitSE.name}");
        }
        else
        {
            Debug.LogWarning("AudioSourceまたはAudioClipが設定されていいない");
        }
        //------------------------------

        if (isKillable && _canOneHitKill)
        {
            //プレイヤーの体力を回復
            player.GetComponent<PlayerController>().StatusManager.TakeDamage(-5);

            //殺す
            //spawnPool.DespawnEnemyFromPool(this.gameObject);
        }

        /*
        if (EnemyStatus.CurrentHealth <= 0)
        {
            spawnPool.DespawnEnemyFromPool(this.gameObject);
        }
        */
    }

    public override void GenerateOnomatopoeia(GameObject _owner, OnomatopoeiaData _onomatopoeiaData)
    {
        if (canGenerateOnomatopoeia)
        {
            //ミッションに追加
            OnomatoMissionManager.Instance.AddToCurData(_onomatopoeiaData);

            GameObject collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject;

            onomatoObj.GetComponent<OnomatopoeiaController>().OwningObject = _owner;
            onomatoObj.GetComponent<OnomatopoeiaController>().Data = _onomatopoeiaData;
            onomatoObj.GetComponent<OnomatopoeiaController>().onomatopoeiaName = _onomatopoeiaData.wordToUse;

            Transform mainCamera = Camera.main.transform;
            //カメラと同じ角度にする
            Quaternion newRotation = mainCamera.rotation;
            //newRotation = newRotation * Quaternion.Euler(0, 0, -90.0f);

            Vector3 boundsOffset = GetComponent<Collider>().bounds.extents;

            Vector3 newPosition = new Vector3(this.transform.position.x + onomatoOffsetPos.x, this.transform.position.y + onomatoOffsetPos.y + boundsOffset.y, this.transform.position.z + onomatoOffsetPos.z - boundsOffset.z);

            GameObject instance = Instantiate(onomatoObj, newPosition, newRotation, collector.transform);
            enemySpawner.enemyOnomatoPool.Add(instance);
            collector.GetComponent<ObjectCollector>().otherObjectPool.Add(instance);
        }
    }

    #region Getter & Setter

    public EnemyStatusHandler Status
    {
        get => enemyStatus;
    }

    public EnemyStateHandler State
    {
        get => enemyState;
    }

    public bool IsKillable
    {
        get => this.isKillable;
        //set { this.isKillable = value; }
    }

    public EnemyAction EnemyAction
    {
        get => this.enemyAction;
    }

    public Animator Anim
    {
        get => this.animator;
    }

    public override Rigidbody RigidBody
    {
        get => this.rigidBody;
    }

    public EnemyAnimManager AnimManager
    {
        get => this.animManager;
    }

    public EnemySkillManager SkillManager
    {
        get => this.skillManager;
    }

    public EnemyUIManager UIManager
    {
        get => this.uIManager;
    }

    public DashHandler DashHandler
    {
        get => this.dashHandler;
    }
    public EnemySpawner EnemySpawner
    {
        get => this.enemySpawner;
        // set { enemySpawner = value; }
    }

    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (Status == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Status.StatusData.chaseDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + new Vector3(0f, 1f, 0f), transform.position + transform.forward * Status.StatusData.chaseDistance);
    }

    private void OnDrawGizmosSelected()
    {
        if (Status == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Status.StatusData.attackDistance);
    }
    #endregion

    #region 今使ってない関数
    /*
    public void TakeDamage()
    {
        if (EnemyStatus.IsDamaged)
        {
            EnemyStatus.IsDamaged = false;
        }
    }

    public void Attack()
    {
        if (AllowAttack && enemyAction)
            State.TransitionState(ObjectStateType.Attack);

        if (!IsAttacking)
        {
            StartCoroutine(nameof(AttackCoroutine));
            AllowAttack = false;
        }
    }
    */
    #endregion
}
