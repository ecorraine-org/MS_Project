using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/WorldObjects/EnemyStatusData", order = 2)]
public class EnemyStatusData : BaseStatusData
{
    [Header("オブジェクトプレハブ")]
    public string gameObjPrefab;

    [Header("エネミー階級")]
    public EnemyRank enemyRank = EnemyRank.None;

    [Header("エネミータイプ")]
    public OnomatoType SelfType = OnomatoType.None;

    [Header("行動クールタイム")]
    public float timeTillNextAction = 1f;

    [Header("攻撃力")]
    public float damage = 1f;

    [Header("攻撃できる距離")]
    public float attackDistance = 2f;

    [Header("移動速度")]
    public float velocity = 1.0f;

    [Header("追跡する距離")]
    public float chaseDistance = 5f;

    [Header("オノマトペデータ")]
    public OnomatopoeiaData onomatoData;

    [Header("耐性")]
    public OnomatoType tolerance = OnomatoType.None;

    [Header("SE")]
    public string sfxClip;

    [Header("VFX")]
    public string vfxClip;


    private void Awake()
    {
        ObjectType = WorldObjectType.Enemy;
        CustomLogger.Log("オブジェクトタイプを初期化");
    }
}
