using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EffectParam
{
    public PlayerEffect effectName;
    public GameObject effectL;//左向き用
    public GameObject effectR;//右向き用
    public Vector3 rotation;
    public Vector3 position;
    public bool isFollow;//ついてくるかどうか

}

[CreateAssetMenu(fileName = "PlayerEffectData", menuName = "ScriptableObjects/Player/EffectData", order = 0)]
public class PlayerEffectData : ScriptableObject
{
    [Header("スキルリスト")]
    public EffectParam[] effects;

    //辞書<キー：スキル種類、値：ステータス>
    public Dictionary<PlayerEffect, EffectParam> dicEffect;

    private void OnEnable()
    {
        dicEffect = new Dictionary<PlayerEffect, EffectParam>();

        //要素追加
        foreach (var effect in effects)
        {
            dicEffect.Add(effect.effectName, effect);
        }

    }
    /// <summary>
    /// ステータス調整をリアルタイムで更新する
    /// </summary>
    private void OnValidate()
    {
        dicEffect = new Dictionary<PlayerEffect, EffectParam>();

        foreach (var effect in effects)
        {
            dicEffect[effect.effectName] = effect;
        }
    }

}

