using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// 敵のステータスを管理するビヘイビア
/// </summary>
public class EnemyStatusManager : StatusManager
{
    [SerializeField, ReadOnly, Tooltip("速度")]
    private float moveSpeed;

    [SerializeField, ReadOnly, Tooltip("攻撃力")]
    private float damage;

    [SerializeField, ReadOnly, Tooltip("攻撃しているか？")]
    private bool isAttack = true;

    [SerializeField, ReadOnly, Tooltip("攻撃されたか？")]
    private bool isDamaged = false;

    [SerializeField, ReadOnly, Tooltip("エネミータイプ")]
    private OnomatoType enemyType;

    [SerializeField, ReadOnly, Tooltip("オノマトペデータ")]
    private OnomatoData onomatoData;

    [SerializeField, ReadOnly, Tooltip("耐性")]
    private OnomatoType tolerance;


    protected override void Awake()
    {
        base.Awake();

        moveSpeed = StatusData.velocity;
        damage = StatusData.fDamage;
        enemyType = StatusData.eEnemyType;
        onomatoData = StatusData.onomatoData;
        tolerance = StatusData.eTolerance;
    }

    public new EnemyStatusData StatusData
    {
        get => (EnemyStatusData)statusData;
    }
}
