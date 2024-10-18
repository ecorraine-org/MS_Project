using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CK_AI : EnemySkill
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

    private EnemySkill enemySkill;

    public Vector3 MovementInput { get; set; }

    private Animator animator;
    private BoxCollider boxCollider;

    //前にツッコむ
    public override void SkillAttack()
    {
        // 俺はもうね。逃げる
        {
            Vector3 movement = -transform.forward * 0.88f * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }
}
