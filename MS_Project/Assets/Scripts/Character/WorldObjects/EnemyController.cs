using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// エネミー用コントローラー
/// </summary>
public class EnemyController : ObjectController
{
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

        battleManager = BattleManager.Instance;

        capsuleCollider = this.GetComponent<CapsuleCollider>();

        spawnPool = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject.GetComponent<Collector>();
    }

    public override void Start()
    {
        base.Start();

        gameObj = Instantiate(Resources.Load<GameObject>(Status.StatusData.gameObjPrefab), this.transform);

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
            enemyAction.EnemyStatus = Status;
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
        /*
        Move();
        Debug.Log(Anim.GetCurrentAnimatorStateInfo(0).shortNameHash.ToString());
        */
    }

    private void Update()
    {
        if (player == null) return;

        //フィニッシュ
        if (Status.Health <= Status.StatusData.maxHealth / 2)
        {
            isKillable = true;
        }

        /*
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < Status.StatusData.chaseDistance)
        {
            Vector3 direction = player.position - transform.position;
            // 進む方向に向く
            Quaternion newRotation = Quaternion.LookRotation(direction.normalized);
            newRotation.x = 0;
            transform.rotation = newRotation;

            if (distance <= Status.StatusData.attackDistance)
            {
                OnMovementInput?.Invoke(Vector3.zero);

                // 攻撃
                OnAttack?.Invoke();
            }
            else
            {
                // 追跡
                OnMovementInput?.Invoke(direction.normalized);
            }
        }
        else
        {
            // 停止
            OnMovementInput?.Invoke(Vector3.zero);
        }

        OnDamaged?.Invoke();
        */
    }

    public void Move()
    {
        if (enemyAction)
            enemyAction.Move();
    }

    public void TakeDamage()
    {
        if (Status.IsDamaged)
        {
            if (enemyAction)
                animator.Play("Damaged");

            GenerateOnomatopoeia();

            Status.IsDamaged = false;
        }
    }

    public void Attack()
    {
        TriggerAttack();

        player.GetComponent<PlayerController>().StatusManager.TakeDamage(Status.Damage);

        StartCoroutine(nameof(AttackCoroutine));

        CanAttack = false;
    }

    public void TriggerAttack()
    {
        if (enemyAction)
        {
            animator.SetTrigger("IsAttack");

            //enemyAction.Attack();
        }
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(Status.ActionCooldown);
        CanAttack = true;
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

        if (Status.Health <= 0)
        {
            spawnPool.DespawnEnemyFromPool(this.gameObject);
        }
    }

    #region Getter & Setter

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
        Gizmos.DrawWireSphere(transform.position, Status.StatusData.attackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Status.StatusData.chaseDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + new Vector3(0f, 1f, 0f), transform.position + transform.forward * Status.StatusData.chaseDistance);
    }
    #endregion
}
