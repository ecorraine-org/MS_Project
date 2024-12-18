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

    //エフェクトデータを格納する変数
    List<PlayerEffectParam> buffEffects = new List<PlayerEffectParam>();

    /// <summary>
    /// バフエフェクト生成
    /// </summary>
    ///     //仕様によって変わる
    public void GenerateBuffEffect(int _index)
    {
        GameObject effectInstance;
        // フェクトを生成
        effectInstance = Instantiate(buffEffects[_index].effectL, transform.TransformPoint(buffEffects[_index].position), transform.rotation * Quaternion.Euler(buffEffects[_index].rotation), buffEffects[_index].isFollow ? transform : null);

        ParticleManager particle = effectInstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.ChangeScale(buffEffects[_index].scale);
            particle.ChangePlaybackSpeed(buffEffects[_index].speed);
            particle.SetStartSize(buffEffects[_index].startSize);

        }


    }

    //仕様によって変わる
    //再び同じバフもらったら、継続時間だけリセットするか、効果を累積するか
    public void SetbuffEffects(int _index, PlayerEffect _effectType)
    {

        while (buffEffects.Count <= _index)
        {
            //要素追加
            buffEffects.Add(new PlayerEffectParam());
        }

        buffEffects[_index] = buffEffectData.dicEffect[_effectType];

    }


    public List<PlayerEffectParam> BuffEffects
    {
        get => buffEffects;
    }
}
