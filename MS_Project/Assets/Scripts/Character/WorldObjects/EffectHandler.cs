using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エフェクト管理
/// </summary>
public class EffectHandler : MonoBehaviour
{
    [SerializeField, Header("被ダメージエフェクト(スプライト)")]
    GameObject spriteHitEffect;

    [SerializeField, Header("被ダメージエフェクト(耐性)")]
    GameObject toleranceHitEffect;

    [SerializeField, Header("被ダメージエフェクト(弱点)")]
    GameObject weakHitEffect;

    [SerializeField, Header("被ダメージエフェクト(ノーマル)")]
    GameObject normalHitEffect;

    [SerializeField, Header("エフェクト生成位置")]
    Transform hitEffectPoint;

    //所有者の参照
    WorldObject owner;

    public void Init(WorldObject _owner)
    {
        owner = _owner;

    }

    //被ダメージエフェクトエフェクト生成
    public void InstantiateSpriteHit()
    {
        Instantiate(spriteHitEffect, hitEffectPoint.position, Quaternion.identity);

    }

    public void InstantiateHit(HitEffect _hitType)
    {
        GameObject curEffect= spriteHitEffect;

        switch (_hitType)
        {
            case HitEffect.Sprite:
                curEffect = spriteHitEffect;
                break;
            case HitEffect.Tolerance:
                curEffect = toleranceHitEffect;
                break;
            case HitEffect.Weakness:
                curEffect = weakHitEffect;
                break;
            case HitEffect.Normal:
                curEffect = normalHitEffect;
                break;
        }

        Instantiate(curEffect, hitEffectPoint.position, Quaternion.identity);
    }
}
