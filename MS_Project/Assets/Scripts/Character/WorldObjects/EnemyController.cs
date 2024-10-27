using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : ObjectController, IHit
{
    [SerializeField, Tooltip("攻撃しているか？")]
    private bool canAttack = true;

    [SerializeField, Tooltip("ラストヒットできるかどうか")]
    protected bool isKillable = false;

    [Header("イベント")]
    public UnityEvent<Vector3> OnMovementInput;
    public UnityEvent OnDamaged;
    public UnityEvent OnAttack;

    private EnemySkill enemySkill;

    public Vector3 MovementInput { get; set; }

    private Animator animator;
    private CapsuleCollider capsuleCollider;
    protected Rigidbody rb;

    public override void Awake()
    {
        base.Awake();

        capsuleCollider = this.GetComponent<CapsuleCollider>();
        rb = this.GetComponent<Rigidbody>();
    }

    public override void Start()
    {
        base.Start();

        gameObj = Instantiate(Resources.Load<GameObject>(status.StatusData.gameObjPrefab), this.transform);
        animator = gameObj.GetComponent<Animator>();

        CapsuleCollider collider = gameObj.GetComponent<CapsuleCollider>();
        capsuleCollider.center = collider.center;
        capsuleCollider.height = collider.height;
        capsuleCollider.radius = collider.radius;

        if (gameObj.TryGetComponent<EnemySkill>(out enemySkill))
        {
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
        {//一旦消してる速度リセ
            //rb.velocity = Vector3.zero;
        }
    }

    public void Attack()
    {
        if (canAttack)
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

        canAttack = false;
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
        canAttack = true;
    }

    public void Hit(bool _canOneHitKill)
    {
        // ヒットストップ
        StartCoroutine(HitStopCoroutine(0.1f, 0.1f));
        

        if (isKillable && _canOneHitKill)
        {
            //プレイヤーの体力を回復
            player.GetComponent<PlayerController>().StatusManager.TakeDamage(-5);

            //殺す
            //Destroy(this.gameObject);
        }
    }

    private IEnumerator HitStopCoroutine(float _slowSpeed, float _duration)
    {
        // 流す速度を遅くする
        animator.speed = _slowSpeed;

        yield return new WaitForSeconds(_duration);

        // 流す速度を戻す
        animator.speed = 1f;
    }

    public bool IsKillable
    {
        get => this.isKillable;
        //set { this.isKillable = value; }
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
