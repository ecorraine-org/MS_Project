using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SkillStatus
{
    public PlayerSkill skillName;
    public float coolTime;
    public float dashSpeed;//突進速度
  //  [Header("突進持続時間 (-1: 時間制限なし)")]
    public float dashDuration;//突進持続時間(-1:時間制限なし)
    public float hpCost;//hp消費量
    public float damage;//攻撃力
    public float slowSpeed;//ヒットストップによる減速
    public float stopDuration;//ヒットストップ持続時間
    public bool canCharge;//長押し(チャージ)可能かどうか
    public float SizeRate;
    public bool canCancel;//回避でキャンセル可能かどうか

}

[CreateAssetMenu(fileName = "PlayerSkillData", menuName = "ScriptableObjects/Player/SkillData", order = 0)]
public class PlayerSkillData : ScriptableObject
{
    [Header("スキルリスト")]
    public SkillStatus[] skill;

    //辞書<キー：スキル種類、値：ステータス>
    public Dictionary<PlayerSkill, SkillStatus> dicSkill;

    private void OnEnable()
    {
        dicSkill = new Dictionary<PlayerSkill, SkillStatus>();

        //要素追加
        foreach (var skillStatus in skill)
        {
            dicSkill.Add(skillStatus.skillName, skillStatus);
        }

    }
    /// <summary>
    /// ステータス調整をリアルタイムで更新する
    /// </summary>
    private void OnValidate()
    {
        dicSkill = new Dictionary<PlayerSkill, SkillStatus>();

        foreach (var skillStatus in skill)
        {
            dicSkill[skillStatus.skillName] = skillStatus;
        }
    }

    //OnValidate
    //public SkillStatus GetSkillStatus(PlayerSkill skillType)
    //{
    //    // スキルタイプが辞書に含まれているか確認
    //    if (dicSkill.TryGetValue(skillType, out SkillStatus status))
    //    {
    //        return status;
    //    }

    //    throw new KeyNotFoundException($"スキルタイプ: {skillType} は辞書に存在しません。");
    //}
}
