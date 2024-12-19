using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーエフェクト管理
/// </summary>
public class PlayerEffectManager : MonoBehaviour
{
    [SerializeField, Header("バフエフェクトデータ")]
    PlayerEffectData buffEffectData;

    //エフェクトを格納する
    GameObject speedBuffinstance;
    GameObject damageBuffinstance;
    GameObject healBuffinstance;

    //PlayerControllerの参照
    PlayerController playerController;

    //エフェクトデータを格納する変数
    // List<PlayerEffectParam> buffEffects = new List<PlayerEffectParam>();

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;

    }

    /// <summary>
    /// バフエフェクト生成
    /// </summary>
    public void GenerateDamageBuffEffect()
    {
        PlayerEffectParam curParam = buffEffectData.dicEffect[PlayerEffect.DamageBuff];

        // フェクトを生成
        damageBuffinstance = Instantiate(curParam.effectL, transform.TransformPoint(curParam.position), transform.rotation * Quaternion.Euler(curParam.rotation), curParam.isFollow ? transform : null);

        ParticleManager particle = damageBuffinstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.ChangeScale(curParam.scale);
            particle.ChangePlaybackSpeed(curParam.speed);
            particle.SetStartSize(curParam.startSize);
            particle.SetLoop(true);
        }
    }

    public void GenerateSpeedBuffEffect()
    {
        PlayerEffectParam curParam = buffEffectData.dicEffect[PlayerEffect.SpeedBuff];

        // フェクトを生成
        speedBuffinstance = Instantiate(curParam.effectL, transform.TransformPoint(curParam.position)  , transform.rotation * Quaternion.Euler(curParam.rotation), curParam.isFollow ? transform : null);

        Debug.Log("エフェクト生成");

        ParticleManager particle = speedBuffinstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.ChangeScale(curParam.scale);
            particle.ChangePlaybackSpeed(curParam.speed);
            particle.SetStartSize(curParam.startSize);
            particle.SetLoop(true);

        }
    }

    public void GenerateHealBuffEffect()
    {
        PlayerEffectParam curParam = buffEffectData.dicEffect[PlayerEffect.HealBuff];

        // フェクトを生成
        healBuffinstance = Instantiate(curParam.effectL, transform.TransformPoint(curParam.position), transform.rotation * Quaternion.Euler(curParam.rotation), curParam.isFollow ? transform : null);

        ParticleManager particle = healBuffinstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.ChangeScale(curParam.scale);
            particle.ChangePlaybackSpeed(curParam.speed);
            particle.SetStartSize(curParam.startSize);
            particle.SetLoop(true);
        }
    }

    public void DestroyHealBuffEffect()
    {
        if (!healBuffinstance) return;

        ParticleManager particle = healBuffinstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.SetLoop(false);
        }
    }

    public void DestroyDamageBuffEffect()
    {
        if (!damageBuffinstance) return;

        ParticleManager particle = damageBuffinstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.SetLoop(false);
        }
    }

    public void DestroySpeedBuffEffect()
    {
        if (!speedBuffinstance) return;

        ParticleManager particle = speedBuffinstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.SetLoop(false);
        }
    }

    /// <summary>
    /// バフエフェクト生成
    /// </summary>
    ///     //仕様によって変わる
    //public void GenerateBuffEffect(int _index)
    //{
    //    GameObject effectInstance;
    //    // フェクトを生成
    //    effectInstance = Instantiate(buffEffects[_index].effectL, transform.TransformPoint(buffEffects[_index].position), transform.rotation * Quaternion.Euler(buffEffects[_index].rotation), buffEffects[_index].isFollow ? transform : null);

    //    ParticleManager particle = effectInstance.GetComponent<ParticleManager>();
    //    if (particle != null)
    //    {
    //        particle.ChangeScale(buffEffects[_index].scale);
    //        particle.ChangePlaybackSpeed(buffEffects[_index].speed);
    //        particle.SetStartSize(buffEffects[_index].startSize);

    //    }


    //}

    //仕様によって変わる
    //再び同じバフもらったら、継続時間だけリセットするか、効果を累積するか
    //public void SetbuffEffects(int _index, PlayerEffect _effectType)
    //{

    //    while (buffEffects.Count <= _index)
    //    {
    //        //要素追加
    //        buffEffects.Add(new PlayerEffectParam());
    //    }

    //    buffEffects[_index] = buffEffectData.dicEffect[_effectType];

    //}


    //public List<PlayerEffectParam> BuffEffects
    //{
    //    get => buffEffects;
    //}
}
