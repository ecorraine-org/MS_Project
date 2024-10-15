using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// 敵のステータスを管理するビヘイビア
/// </summary>
public class ObjectStatusManager : StatusManager
{
    [Tooltip("攻撃されたか？")]
    private bool isDamaged = false;

    [Tooltip("生きているか？")]
    private bool isAlive = true;

    [SerializeField, Tooltip("速度")]
    private float moveSpeed;

    [SerializeField, Tooltip("攻撃力")]
    private float damage;

    [SerializeField, Tooltip("自分の属性")]
    private OnomatoType selfType;

    [SerializeField, Tooltip("オノマトペデータ")]
    private OnomatopoeiaData onomatoData;

    [SerializeField, Tooltip("耐性")]
    private OnomatoType tolerance;


    protected override void Awake()
    {
        base.Awake();

        moveSpeed = StatusData.velocity;
        damage = StatusData.damage;
        selfType = StatusData.SelfType;
        onomatoData = StatusData.onomatoData;
        tolerance = StatusData.tolerance;
    }

    public override void TakeDamage(float _damage)
    {
        isDamaged = true;
        base.TakeDamage(_damage);
    }

    public new EnemyStatusData StatusData
    {
        get => (EnemyStatusData)statusData;
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
