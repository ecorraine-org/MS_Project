﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// プレイヤースキル管理のビヘイビア
/// </summary>
public class PlayerSkillManager : MonoBehaviour
{
    //PlayerControllerの参照
    PlayerController playerController;

    [SerializeField, Header("ステータスデータ")]
    PlayerSkillData skillData;

  //  [SerializeField, Header("今のスキル")]
  //  PlayerSkill curSkill;

    [SerializeField, Header("今のスキルのクールタイム")]
    float curSkillCoolTime;

    //辞書<キー：スキル種類、値：クールタイム>
    private Dictionary<PlayerSkill, float> coolTimers = new Dictionary<PlayerSkill, float>();


    private void Awake()
    {
        coolTimers.Add(PlayerSkill.Eat, 0f);
        coolTimers.Add(PlayerSkill.Sword, 0f);
        coolTimers.Add(PlayerSkill.Hammer, 0f);
    }

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;

    }


    /// <summary>
    /// スキル種類に応じて、今のスキルを設定する
    /// </summary>
    //public void SetCurSkill(PlayerMode _mode)
    //{
    //    PlayerSkill skill = (PlayerSkill)(int)_mode;

    //    //有効かをチェック
    //    if (Enum.IsDefined(typeof(PlayerSkill), skill))
    //    {
    //        curSkill = skill;

    //        skillData.curSkillCoolTime = curSkill switch
    //        {
    //            PlayerSkill.Sword => skillData.swordSkillCoolTime,
    //            PlayerSkill.Hammer => skillData.hammerSkillCoolTime,
    //            //デフォルト
    //            _ => 0f
    //        };
    //    }

    //}

    ///// <summary>
    ///// 今のモードに応じるスキルを発動する
    ///// </summary>
    //public void UseCurSkill()
    //{
    //    UseSkill(PlayerSkill.CurSkill); 
    //}

    /// <summary>
    /// スキル種類に応じて、スキルを発動する
    /// </summary>
    public void UseSkill(PlayerSkill _skillType)
    {
        Debug.Log($"{_skillType}クールタイム中");//test
        if (coolTimers.ContainsKey(_skillType) && coolTimers[_skillType] <= 0f)
        {
            Debug.Log($"{_skillType}発動");//test

            //スキル発動
            ExecuteSkill(_skillType);
       
            //クールタイム処理
            StartCoroutine(SkillCooldown( _skillType));
        }
    }

    public void ExecuteSkill(PlayerSkill _skillType)
    {
        switch (_skillType)
        {
            case PlayerSkill.Eat:

                ExecuteEat();
                break;
            case PlayerSkill.Sword:
   
                ExecuteSwordSkill();
                break;
            case PlayerSkill.Hammer:
  
                ExecuteHammerSkill();
                break;
            default:
                break;
        }
    }

    private void ExecuteEat()
    {
        playerController.SpriteAnim.Play("Eat");

        Debug.Log("Eat skill executed!");
    }

    private void ExecuteSwordSkill()
    {
        playerController.SpriteAnim.Play("Attack");
        playerController.SpriteRenderer.color = Color.red;

        Debug.Log("Sword skill executed!");
    }

    private void ExecuteHammerSkill()
    {
        playerController.SpriteAnim.Play("HammerAttack");
        playerController.SpriteRenderer.color = Color.red;
    }


    private IEnumerator SkillCooldown( PlayerSkill _skillType)
    {
        float coolTime = _skillType switch
        {
            PlayerSkill.Eat => skillData.eatingCoolTime,
            PlayerSkill.Sword => skillData.swordSkillCoolTime,
            PlayerSkill.Hammer => skillData.hammerSkillCoolTime,
            //デフォルト
            _ => 0f
        };

        //クールタイム設定
        coolTimers[_skillType] = coolTime;

        while (coolTimers[_skillType] > 0f)
        {
            coolTimers[_skillType] -= Time.deltaTime;

            // クールタイムを0以上にする
            coolTimers[_skillType] = Mathf.Max(coolTimers[_skillType], 0f);

            //スキルクールタイムを記録  
             curSkillCoolTime = coolTimers[_skillType];

            yield return null;
        }

        Debug.Log($"{_skillType} スキルクールダウン完了");
    }

    public IReadOnlyDictionary<PlayerSkill, float> CoolTimers
    {
        get => coolTimers; 
    }
}

