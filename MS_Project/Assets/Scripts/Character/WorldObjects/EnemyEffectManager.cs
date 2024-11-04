using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エフェクト管理
/// </summary>
public class EnemyEffectManager : MonoBehaviour
{
    [SerializeField, Header("被ダメージエフェクト")]
    GameObject hitEffect;

    [SerializeField, Header("エフェクト生成位置")]
    Transform hitEffectPoint;

    //EnemyControllerの参照
    EnemyController enemyController;

    public void Init(EnemyController _enemy)
    {
        enemyController = _enemy;

    }

    //被ダメージエフェクトエフェクト生成
    public void InstantiateHit()
    {
        Instantiate(hitEffect, hitEffectPoint.position, Quaternion.identity);

    }
}
