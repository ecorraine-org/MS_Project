using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : ObjectController, IHit
{
    //シングルトン
    BattleManager battleManager;

    [SerializeField, Header("被ダメージエフェクト")]
    GameObject hitEffect;

    //アニメーションマネージャー
    EnemyAnimManager animManager;

    [SerializeField, Tooltip("ラストヒットできるかどうか")]
    protected bool isKillable = false;

    [Header("イベント")]
    public UnityEvent<Vector3> OnMovementInput;
    public UnityEvent OnDamaged;
    public UnityEvent OnAttack;
    public Vector3 MovementInput { get; set; }

    private EnemyAction enemySkill;

    private Animator animator;
    private CapsuleCollider capsuleCollider;
    private Rigidbody rb;

    private GameObject spawnPool;

    public override void Awake()
    {
        base.Awake();

        battleManager = BattleManager.Instance;

        capsuleCollider = this.GetComponent<CapsuleCollider>();
        rb = this.GetComponent<Rigidbody>();

        spawnPool = GameObject.FindGameObjectWithTag("EnemyCollector").gameObject;
    }

    public override void Start()
    {
        base.Start();

        gameObj = Instantiate(Resources.Load<GameObject>(status.StatusData.gameObjPrefab), this.transform);
        animator = gameObj.GetComponent<Animator>();

        animManager = gameObj.GetComponentInChildren<EnemyAnimManager>();
        animManager.Init(this);

        CapsuleCollider collider = gameObj.GetComponent<CapsuleCollider>();
        capsuleCollider.center = collider.center;
        capsuleCollider.height = collider.height;
        capsuleCollider.radius = collider.radius;

        if (gameObj.TryGetComponent<EnemyAction>(out enemySkill))
        {
            enemySkill.EnemyStatus = status;
            enemySkill.SetAnimator(animator);
            enemySkill.SetPlayer(player);
            enemySkill.SetRigidbody(rb);
        }
        else
        {
            CustomLogger.LogWarning(gameObj.GetType(), gameObj.name);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        if (player == null) return;

         //フィニッシュ
        if (status.Health <= status.StatusData.maxHealth / 2)
        {
            isKillable = true;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < status.StatusData.chaseDistance)
        {
                Vector3 direction = player.position - transform.position;
                // 進む方向に向く
                Quaternion newRotation = Quaternion.LookRotation(direction.normalized);
                newRotation.x = 0;
                transform.rotation = newRotation;

            if (distance <= status.StatusData.attackDistance)
            {
                OnMovementInput?.Invoke(Vector3.zero);
                if (enemySkill)
                {
                    enemySkill.SetDistanceToPlayer(distance);
                }
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
    }

    private void Move()
    {
        if (MovementInput.magnitude > 0.1f && status.MoveSpeed >= 0)
        {
            animator.Play("Walk");
            // 前に進む
            rb.velocity = MovementInput * status.MoveSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void Attack()
    {
        if (CanAttack)
        {
            UseSkill();

            player.GetComponent<PlayerController>().StatusManager.TakeDamage(status.Damage);

            StartCoroutine(nameof(AttackCoroutine));
        }
    }

    public void UseSkill()
    {
        animator.SetTrigger("IsAttack");
        if (enemySkill)
            enemySkill.SkillAttack();

        CanAttack = false;
    }

    public void TakeDamage()
    {
        if (status.IsDamaged)
        {
            animator.Play("Damaged",0,0f);

            GenerateOnomatopoeia();

            status.IsDamaged = false;
        }
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(status.ActionCooldown);
        CanAttack = true;
    }

    public void Hit(bool _canOneHitKill)
    {
        HitReaction hitReaction = battleManager.GetPlayerHitReaction();
        // ヒットストップ          
        battleManager.StartHitStop(animator);

        Instantiate(hitEffect, transform.position, Quaternion.identity);


        if (isKillable && _canOneHitKill)
        {
            //プレイヤーの体力を回復
            player.GetComponent<PlayerController>().StatusManager.TakeDamage(-5);

            //殺す
            spawnPool.GetComponent<EnemySpawner>().DespawnEnemy(this.gameObject);
        }

        if (status.Health <= 0)
        {
            spawnPool.GetComponent<EnemySpawner>().DespawnEnemy(this.gameObject);
        }
    }

    public bool IsKillable
    {
        get => this.isKillable;
        //set { this.isKillable = value; }
    }

    public override Rigidbody RigidBody
    {
        get => rb;
    }

    public new EnemyAnimManager AnimManager
    {
        get => this.animManager;
    }

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, status.StatusData.attackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, status.StatusData.chaseDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + new Vector3(0f, 1f, 0f), transform.position + transform.forward * status.StatusData.chaseDistance);
    }
    #endregion
}
