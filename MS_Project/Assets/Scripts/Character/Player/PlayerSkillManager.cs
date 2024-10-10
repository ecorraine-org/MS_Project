using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// プレイヤースキル管理のビヘイビア
/// </summary>
public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField, Header("ステータスデータ")]
    PlayerSkillData skillData;

    [SerializeField, Header("今のスキル")]
    PlayerSkill curSkill;

    [SerializeField, Header("今のスキルのクールタイム")]
    float curSkillCoolTime;

    //辞書<キー：スキル種類、値：クールタイム>
    private Dictionary<PlayerSkill, float> coolTimers = new Dictionary<PlayerSkill, float>();

    private void Awake()
    {
        coolTimers.Add(PlayerSkill.Eat, 0f);
        coolTimers.Add(PlayerSkill.Sword, 0f);
        coolTimers.Add(PlayerSkill.Hammer, 0f);
        coolTimers.Add(PlayerSkill.CurSkill, 0f);
    }

    /// <summary>
    /// スキル種類に応じて、今のスキルを設定する
    /// </summary>
    public void SetCurSkill(PlayerMode _mode)
    {
        PlayerSkill skill = (PlayerSkill)(int)_mode;

        //有効かをチェック
        if (Enum.IsDefined(typeof(PlayerSkill), skill))
        {
            curSkill = skill;

            skillData.curSkillCoolTime = curSkill switch
            {
                PlayerSkill.Sword => skillData.swordSkillCoolTime,
                PlayerSkill.Hammer => skillData.hammerSkillCoolTime,
                //デフォルト
                _ => 0f
            };
        }
    
    }

    /// <summary>
    /// 今のモードに応じるスキルを発動する
    /// </summary>
    public void UseCurSkill()
    {
        UseSkill(PlayerSkill.CurSkill); 
    }

    /// <summary>
    /// スキル種類に応じて、スキルを発動する
    /// </summary>
    public void UseSkill(PlayerSkill _skillType)
    {
        Debug.Log($"{_skillType}クールタイム中");//test
        if (coolTimers.ContainsKey(_skillType) && coolTimers[_skillType] <= 0f)
        {
            float coolTime = _skillType switch
            {
                PlayerSkill.Eat => skillData.eatingCoolTime,
                //PlayerSkill.Sword => skillData.swordSkillCoolTime,
                //PlayerSkill.Hammer => skillData.hammerSkillCoolTime,
                PlayerSkill.CurSkill => skillData.curSkillCoolTime,
                //デフォルト
                _ => 0f
            };

            StartCoroutine(SkillCooldown(coolTime, _skillType));
        }
    }



    private IEnumerator SkillCooldown(float _coolTime, PlayerSkill _skillType)
    {
        //クールタイム設定
        coolTimers[_skillType] = _coolTime;

        while (_coolTime > 0f)
        {
            coolTimers[_skillType] -= Time.deltaTime;

            // クールタイムを0以上にする
            coolTimers[_skillType] = Mathf.Max(coolTimers[_skillType], 0f);

            //スキルクールタイムを記録  
            if(_skillType==PlayerSkill.CurSkill) curSkillCoolTime = coolTimers[_skillType];

            yield return null;
        }

        Debug.Log($"{_skillType} スキルクールダウン完了");
    }
}


