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


    [SerializeField, Header("エフェクトデータ")]
    EnemyEffectData effectData;

    //[SerializeField, NonEditable, Header("エフェクトを格納する変数")]
    //GameObject curEffect;

    //生成したエフェクトを格納する
    GameObject effectInstance;

    //エフェクトデータを格納する変数
    List<EnemyEffectParam> curEffectParams = new List<EnemyEffectParam>();

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
        GameObject curHitEffect = spriteHitEffect;

        switch (_hitType)
        {
            case HitEffect.Sprite:
                curHitEffect = spriteHitEffect;
                break;
            case HitEffect.Tolerance:
                curHitEffect = toleranceHitEffect;
                break;
            case HitEffect.Weakness:
                curHitEffect = weakHitEffect;
                break;
            case HitEffect.Normal:
                curHitEffect = normalHitEffect;
                break;
        }

        Instantiate(curHitEffect, hitEffectPoint.position, Quaternion.identity);
    }

    /// <summary>
    /// アニメーションイベントに呼び出される
    /// スキルエフェクト生成
    /// </summary>
    public void GenerateEffect(int _index)
    {
        // フェクトを生成
        effectInstance = Instantiate(curEffectParams[_index].effect, transform.TransformPoint(curEffectParams[_index].position), transform.rotation * Quaternion.Euler(curEffectParams[_index].rotation), curEffectParams[_index].isFollow ? transform : null);

        ParticleManager particle = effectInstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.ChangeScale(curEffectParams[_index].scale);
            particle.ChangePlaybackSpeed(curEffectParams[_index].speed);
            //  particle.SetStartSize(curEffectParam[_index].startSize);

        }


    }

    public void SetCurEffectParam(int _index, EnemyEffect _effectType)
    {

        while (curEffectParams.Count <= _index)
        {
            //要素追加
            curEffectParams.Add(new EnemyEffectParam());
        }

        curEffectParams[_index] = effectData.dicEffect[_effectType];

    }


    public List<EnemyEffectParam> CurEffectParam
    {
        get => curEffectParams;
    }
}
