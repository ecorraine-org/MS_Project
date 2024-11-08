using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/WorldObjects/EnemyStatusData", order = 2)]
public class EnemyStatusData : ObjectStatusData
{
    [Header("オブジェクトタイプ")]
    public readonly WorldObjectType objectType = WorldObjectType.Enemy;

    [Header("エネミー階級")]
    public EnemyRank enemyRank = EnemyRank.None;

    [Header("攻撃力")]
    public float damage = 1f;

    [Header("攻撃できる距離")]
    public float attackDistance = 2f;

    [Header("移動速度")]
    public float velocity = 1.0f;

    [Header("追跡する距離")]
    public float chaseDistance = 5f;
}
