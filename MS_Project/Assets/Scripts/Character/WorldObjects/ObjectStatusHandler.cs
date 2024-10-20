using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// 敵のステータスを管理するビヘイビア
/// </summary>
public class ObjectStatusHandler : StatusManager
{
    [Tooltip("攻撃されたか？")]
    private bool isDamaged = false;

    [Tooltip("生きているか？")]
    private bool isAlive = true;

    [SerializeField, Tooltip("速度")]
    private float moveSpeed;

    [SerializeField, Tooltip("攻撃力")]
    private float damage;

    [SerializeField, Tooltip("行動クールタイム")]
    private float actionCooldown;

    [SerializeField, Tooltip("自分の属性")]
    private OnomatoType selfType;

    [SerializeField, Tooltip("オノマトペデータ")]
    private OnomatopoeiaData onomatoData;

    [SerializeField, Tooltip("耐性")]
    private OnomatoType tolerance;


    EnemyStatusData enemyStatusData;

    protected override void Awake()
    {
        //base.Awake();

        //enemyStatusData = ScriptableObject.CreateInstance<EnemyStatusData>();
    }

    protected virtual void Start()
    {
        statusData = enemyStatusData;
        currentHealth = enemyStatusData.maxHealth;
        moveSpeed = enemyStatusData.velocity;
        damage = enemyStatusData.damage;
        actionCooldown = enemyStatusData.timeTillNextAction;
        selfType = enemyStatusData.SelfType;
        onomatoData = enemyStatusData.onomatoData;
        tolerance = enemyStatusData.tolerance;
    }

    public override void TakeDamage(float _damage)
    {
        isDamaged = true;
        base.TakeDamage(_damage);
    }

    public new EnemyStatusData StatusData
    {
        get => (EnemyStatusData)enemyStatusData;
        set { enemyStatusData = value; }
    }

    public float MoveSpeed
    {
        get => moveSpeed;
        set { moveSpeed = value; }
    }

    public float Damage
    {
        get => damage;
        set { damage = value; }
    }

    public float ActionCooldown
    {
        get => actionCooldown;
        set { actionCooldown = value; }
    }

    public bool IsDamaged
    {
        get => isDamaged;
        set { isDamaged = value; }
    }

    public bool IsAlive
    {
        get => isAlive;
        set { isAlive = value; }
    }
}
