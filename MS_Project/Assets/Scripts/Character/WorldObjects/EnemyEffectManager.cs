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

    //所有者の参照
    WorldObject owner;

    public void Init(WorldObject _owner)
    {
        owner = _owner;

    }

    //被ダメージエフェクトエフェクト生成
    public void InstantiateHit()
    {
        Instantiate(hitEffect, hitEffectPoint.position, Quaternion.identity);

    }
}
