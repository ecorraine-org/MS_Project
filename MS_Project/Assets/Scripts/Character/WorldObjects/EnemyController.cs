using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// エネミー用コントローラー
/// </summary>
public class EnemyController : WorldObjectController
{
    [HideInInspector, Tooltip("エネミーステータスマネージャー")]
    EnemyStatusHandler enemyStatus;

    // シングルトン
    BattleManager battleManager;

    // アニメーションマネージャー
    EnemyAnimManager animManager;

    // スキルマネージャー
    EnemySkillManager skillManager;

    // エフェクトマネージャー
    EnemyEffectManager effectManager;

    [SerializeField, Tooltip("ラストヒットできるかどうか？")]
    protected bool isKillable = false;

    private EnemyAction enemyAction;

    private Animator animator;
    private CapsuleCollider capsuleCollider;

    private Collector spawnPool;

    public override void Awake()
    {
        base.Awake();

        if (!this.transform.GetChild(0).gameObject.TryGetComponent<EnemyStatusHandler>(out enemyStatus))
            CustomLogger.LogWarning(EnemyStatus.GetType(), EnemyStatus.name);

        battleManager = BattleManager.Instance;

        capsuleCollider = this.GetComponent<CapsuleCollider>();

        spawnPool = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject.GetComponent<Collector>();
    }

    public void Start()
    {
        gameObj = Instantiate(Resources.Load<GameObject>(EnemyStatus.StatusData.gameObjPrefab), this.transform);

        rigidBody.mass = EnemyStatus.StatusData.mass;
        type = EnemyStatus.StatusData.ObjectType;

        animator = gameObj.GetComponentInChildren<Animator>();

        animManager = GetComponentInChildren<EnemyAnimManager>();
        animManager.Init(this);

        skillManager = GetComponentInChildren<EnemySkillManager>();
        skillManager.Init(this);

        effectManager = GetComponentInChildren<EnemyEffectManager>();
        effectManager.Init(this);

        CapsuleCollider collider = gameObj.GetComponent<CapsuleCollider>();
        capsuleCollider.center = collider.center;
        capsuleCollider.height = collider.height;
        capsuleCollider.radius = collider.radius;

        enemyAction = gameObj.GetComponentInChildren<EnemyAction>();
        if (enemyAction)
        {
            enemyAction.Enemy = this;
            enemyAction.EnemyStatus = EnemyStatus;
        }
        else
        {
            CustomLogger.LogWarning(gameObj.GetType(), gameObj.name);
        }

        State = GetComponentInChildren<ObjectStateHandler>();
        State.Init(this);
    }

    private void FixedUpdate()
    {
        Move();

        Vector3 gravity = Physics.gravity * (rigidBody.mass * rigidBody.mass);
        rigidBody.AddForce(gravity * Time.deltaTime);
    }

    private void Update()
    {
        if (player == null) return;

        // フィニッシュ
        if (EnemyStatus.CurrentHealth <= EnemyStatus.StatusData.maxHealth / 2)
        {
            isKillable = true;
        }

        Chase();
        /*
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= EnemyStatus.StatusData.chaseDistance)
        {
            Vector3 direction = player.position - transform.position;
            // 進む方向に向く
            Quaternion newRotation = Quaternion.LookRotation(direction.normalized);
            newRotation.x = 0f;
            transform.rotation = newRotation;

            if (distance <= EnemyStatus.StatusData.attackDistance)
            {
                OnMovementInput?.Invoke(Vector3.zero);

                // 攻撃
                OnAttack?.Invoke();
            }
            else
            {
                IsAttacking = false;
                // 追跡
                OnMovementInput?.Invoke(direction.normalized);
            }
        }
        else
        {
            IsAttacking = false;
            // 停止
            OnMovementInput?.Invoke(Vector3.zero);
            State.TransitionState(ObjectStateType.Idle);
        }
        */

        OnDamaged?.Invoke();
    }

    public void TakeDamage()
    {
        if (EnemyStatus.IsDamaged)
        {
            if (enemyAction)
                animator.Play("Damaged");

            GenerateOnomatopoeia(enemyStatus.StatusData.onomatoData);

            EnemyStatus.IsDamaged = false;

            State.TransitionState(ObjectStateType.Damaged);
        }
    }

    public void Move()
    {
        State.TransitionState(ObjectStateType.Walk);
        /*
        if (MovementInput.magnitude > 0.1f && EnemyStatus.MoveSpeed > 0)
            RigidBody.velocity = MovementInput * EnemyStatus.MoveSpeed;
        */
    }

    public void Chase()
    {
        if (enemyAction)
            enemyAction.Chase();
    }

    public void Attack()
    {
        if (IsAttacking && enemyAction)
            State.TransitionState(ObjectStateType.Attack);

        player.GetComponent<PlayerController>().StatusManager.TakeDamage(EnemyStatus.Damage);

        StartCoroutine(nameof(AttackCoroutine));

        IsAttacking = false;
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(EnemyStatus.ActionCooldown);
        IsAttacking = true;
    }

    public override void Hit(bool _canOneHitKill)
    {
        HitReaction hitReaction = battleManager.GetPlayerHitReaction();
        // ヒットストップ
        battleManager.StartHitStop(animator);

        //エフェクト生成
        effectManager.InstantiateHit();

        if (isKillable && _canOneHitKill)
        {
            //プレイヤーの体力を回復
            player.GetComponent<PlayerController>().StatusManager.TakeDamage(-5);

            //殺す
            spawnPool.DespawnEnemyFromPool(this.gameObject);
        }

        if (EnemyStatus.CurrentHealth <= 0)
        {
            spawnPool.DespawnEnemyFromPool(this.gameObject);
        }
    }

    #region Getter & Setter

    public EnemyStatusHandler EnemyStatus
    {
        get => enemyStatus;
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

    public EnemyEffectManager EffectManager
    {
        get => this.effectManager;
    }

    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnemyStatus.StatusData.attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, EnemyStatus.StatusData.chaseDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + new Vector3(0f, 1f, 0f), transform.position + transform.forward * EnemyStatus.StatusData.chaseDistance);
    }
    #endregion
}
