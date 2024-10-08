using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("プレイヤー")]
    Transform player;
    [SerializeField, Header("ステータスマネージャー")]
    EnemyStatusManager status;

    [Header("イベント")]
    public UnityEvent<Vector3> OnMovementInput;
    public UnityEvent OnAttack;
    public UnityEvent OnDamaged;

    [SerializeField, Header("ステータス"), Tooltip("攻撃力")]
    float fDamage = 0;
    [SerializeField, Tooltip("速度")]
    float currentSpeed = 0;
    [SerializeField, Tooltip("攻撃しているか？")]
    bool isAttack = true;
    [SerializeField, Tooltip("攻撃クールタイム")]
    float attackCoolDuration = 1;
    //[SerializeField, Tooltip("攻撃されたか？")]
    //bool isDamaged = false;

    public Vector3 MovementInput { get; set; }

    [SerializeField]
    GameObject onomatoObj;
    Rigidbody rb;
    Animator animator;
    BoxCollider boxCollider;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        status = gameObject.transform.GetChild(0).gameObject.GetComponent<EnemyStatusManager>();

        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(1).GetComponent<Animator>();
    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        BoxCollider collider = gameObject.transform.GetChild(1).gameObject.GetComponent<BoxCollider>();
        boxCollider.center = collider.center / 3;
        boxCollider.size = collider.size / 3;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < status.StatusData.fChaseDistance)
        {

            if (distance <= status.StatusData.fAttackDistance)
            {
                OnMovementInput?.Invoke(Vector3.zero);
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
            //  Debug.Log("敵攻撃");
            animator.Play("Attack");
            isAttack = false;
            StartCoroutine(nameof(AttackCoroutine));
        }
    }

    public void TakeDamage()
    {
        if (status.isDamaged)
        {
            animator.Play("Damaged");
            Debug.Log("ダメージされた");
            GameObject instance = Instantiate(onomatoObj, this.transform);
            status.isDamaged = false;
        }
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackCoolDuration);
        isAttack = true;
    }
}
