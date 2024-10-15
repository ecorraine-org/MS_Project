using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : ObjectController, IHit
{
    public readonly WorldObjectType type = WorldObjectType.Enemy;

    [SerializeField, Header("ステータス"), Tooltip("攻撃力")]
    float fDamage = 0;
    [SerializeField, Tooltip("速度")]
    float currentSpeed = 0;
    [SerializeField, Tooltip("攻撃しているか？")]
    bool isAttack = true;
    [SerializeField, Tooltip("攻撃クールタイム")]
    float attackCoolDuration = 1;

    [Header("イベント")]
    public UnityEvent<Vector3> OnMovementInput;
    public UnityEvent OnDamaged;
    public UnityEvent OnAttack;
    public UnityEvent OnUseSkill;

    public EnemySkill enemySkill;

    public Vector3 MovementInput { get; set; }

    private Animator animator;
    private BoxCollider boxCollider;
    protected Rigidbody rb;

    public override void Awake()
    {
        base.Awake();

        gameObj = Instantiate(Resources.Load<GameObject>(status.StatusData.gameObjPrefab), this.transform);
        animator = this.transform.GetChild(1).GetComponent<Animator>();

        rb = this.GetComponent<Rigidbody>();
    }

    public override void Start()
    {
        base.Start();

        boxCollider = this.GetComponent<BoxCollider>();
        BoxCollider collider = this.gameObject.transform.GetChild(1).gameObject.GetComponent<BoxCollider>();
        boxCollider.center = collider.center / 3;
        boxCollider.size = collider.size / 3;

        enemySkill = this.gameObject.transform.GetChild(1).gameObject.GetComponent<EnemySkill>();
        enemySkill.SetRigidbody(rb);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < status.StatusData.chaseDistance)
        {

            if (distance <= status.StatusData.attackDistance)
            {
                OnMovementInput?.Invoke(Vector3.zero);
                // 攻撃
                OnAttack?.Invoke();
                OnUseSkill?.Invoke();
            }
            else
            {
                // 追跡
                Vector3 direction = player.position - transform.position;
                // 進む方向に向く
                Quaternion newRotation = Quaternion.LookRotation(direction.normalized);
                newRotation.x = 0;
                transform.rotation = newRotation;

                OnMovementInput?.Invoke(direction.normalized);
            }
        }
        else
        {
            // 停止
            OnMovementInput?.Invoke(Vector3.zero);
        }

        OnDamaged?.Invoke();

        if (Debug.isDebugBuild)
        {
            Debug.DrawRay(transform.position, (player.position - transform.position) * 2, Color.blue);
            Debug.DrawRay(transform.position, transform.forward * 3, Color.red);
        }
    }

    private void Move()
    {
        if (MovementInput.magnitude > 0.1f && currentSpeed >= 0)
        {
            animator.Play("Walk");
            // 前に進む
            rb.velocity = MovementInput * currentSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void Attack()
    {
        if (isAttack)
        {
            animator.SetTrigger("IsAttack");

            player.GetComponent<PlayerController>().StatusManager.TakeDamage(status.StatusData.damage);

            isAttack = false;
            StartCoroutine(nameof(AttackCoroutine));
        }
    }

    public void UseSkill()
    {
        enemySkill.SkillAttack();
    }

    public void TakeDamage()
    {
        if (status.IsDamaged)
        {
            animator.Play("Damaged");

            GenerateOnomatopoeia();

            status.IsDamaged = false;
        }
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackCoolDuration);
        isAttack = true;
    }

    public void Hit(bool _canOneHitKill)
    {

    }
}