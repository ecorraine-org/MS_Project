using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyEffectParam
{
    public EnemyEffect effectName;
    public GameObject effect;
    public Vector3 rotation;
    public Vector3 position;
    public Vector3 scale;
  //  public Vector2 startSize;
    public float speed;
    public bool isFollow;//ついてくるかどうか

}

[CreateAssetMenu(fileName = "EnemyEffectData", menuName = "ScriptableObjects/Enemy/EffectData", order = 0)]
public class EnemyEffectData : ScriptableObject
{
    [Header("スキルリスト")]
    public EnemyEffectParam[] effects;

    //辞書<キー：スキル種類、値：ステータス>
    public Dictionary<EnemyEffect, EnemyEffectParam> dicEffect;

    private void OnEnable()
    {
        dicEffect = new Dictionary<EnemyEffect, EnemyEffectParam>();

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
        dicEffect = new Dictionary<EnemyEffect, EnemyEffectParam>();

        foreach (var effect in effects)
        {
            dicEffect[effect.effectName] = effect;
        }
    }

}

