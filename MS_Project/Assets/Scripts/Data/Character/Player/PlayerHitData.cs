using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HitReaction
{
    public PlayerMode mode;
    public OnomatoType onomatoType;
    public float slowSpeed;//ヒットストップによる減速
    public float stopDuration;//ヒットストップ持続時間
    public float moveSpeed;//攻撃中進む速度
    public float damage;//攻撃力
}

/// <summary>
/// プレイヤーのヒット感に関する設定
/// </summary>
[CreateAssetMenu(fileName = "PlayerHitData", menuName = "ScriptableObjects/Player/HitData", order = 0)]
public class PlayerHitData : ScriptableObject
{
    [Header("スキルリスト")]
    public HitReaction[] hitReac;

    //辞書<キー：スキル種類、値：ステータス>
    public Dictionary<PlayerMode, HitReaction> dicHitReac;

    private void OnEnable()
    {
        dicHitReac = new Dictionary<PlayerMode, HitReaction>();

        //要素追加
        foreach (var reaction in hitReac)
        {
            dicHitReac.Add(reaction.mode, reaction);
        }

    }
    /// <summary>
    /// ステータス調整をリアルタイムで更新する
    /// </summary>
    private void OnValidate()
    {
        dicHitReac = new Dictionary<PlayerMode, HitReaction>();

        foreach (var reaction in hitReac)
        {
            dicHitReac[reaction.mode] = reaction;
        }
    }
}
