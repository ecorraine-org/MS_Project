using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : ObjectController, IHit
{
    [SerializeField, Tooltip("攻撃しているか？")]
    private bool canAttack = true;

    [Header("イベント")]
    public UnityEvent<Vector3> OnMovementInput;
    public UnityEvent OnDamaged;
    public UnityEvent OnAttack;

    private EnemySkill enemySkill;

    public Vector3 MovementInput { get; set; }

    private Animator animator;
    private SphereCollider sphereCollider;
    protected Rigidbody rb;

    public override void Awake()
    {
        base.Awake();

        sphereCollider = this.GetComponent<SphereCollider>();
        rb = this.GetComponent<Rigidbody>();
    }

    public override void Start()
    {
        base.Start();

        gameObj = Instantiate(Resources.Load<GameObject>(status.StatusData.gameObjPrefab), this.transform);
        animator = gameObj.GetComponent<Animator>();

        SphereCollider collider = gameObj.GetComponent<SphereCollider>();
        sphereCollider.center = collider.center;
        sphereCollider.radius = collider.radius / 3;

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

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < status.StatusData.chaseDistance)
        {

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
            animator.Play("Damaged");

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

    }
}