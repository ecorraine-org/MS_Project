using System.Collections;
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

    [SerializeField, Header("突進処理ビヘイビア")]
    DashHandler dash;

    [SerializeField, Header("ステータスデータ")]
    PlayerSkillData skillData;

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

        dash.Init(playerController, skillData);

    }

    /// <summary>
    /// スキル発動するためのhp消費
    /// </summary>
    public bool HpCost(PlayerSkill _skillType)
    {
        // スキルのHPコストが現在のHPより少ないか
        if (skillData.dicSkill[_skillType].hpCost < playerController.StatusManager.Health)
        {
            var life = playerController.GetComponentInChildren<ILife>();
            if (life != null)
            {
                //hp消費
                life.TakeDamage(skillData.dicSkill[_skillType].hpCost);
            }

            return true;
        }
       

        return false;
    }

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
            StartCoroutine(SkillCooldown(_skillType));
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
            case PlayerSkill.Spear:

                ExecuteSpearSkill();
                break;
            case PlayerSkill.Gauntlet:

                ExecuteGauntletSkill();
                break;
            default:
                break;
        }
    }

    //キャンセルされた時のリセット処理
    public void Reset()
    {
        if (dash.IsDashing) dash.EndDash();
    }

    public void ExecuteDodge(bool _canThrough, Vector3 _direc = default)
    {
        //突進初期化
        dash.Speed = skillData.dicSkill[PlayerSkill.Dodge].dashSpeed;
        dash.Duration = skillData.dicSkill[PlayerSkill.Dodge].dashDuration;

        dash.StartDash(_canThrough, _direc);
    }

    public void ExecuteEat()
    {
        playerController.SpriteAnim.Play("Eat");

        Debug.Log("Eat skill executed!");
    }

    private void ExecuteSwordSkill()
    {
        playerController.SpriteAnim.Play("SwordSkill");
        playerController.SpriteRenderer.color = Color.red;

        //突進初期化
        dash.Speed = skillData.dicSkill[PlayerSkill.Sword].dashSpeed;
        dash.Duration = skillData.dicSkill[PlayerSkill.Sword].dashDuration;

        Debug.Log("Sword skill executed!");
    }

    private void ExecuteHammerSkill()
    {
        playerController.SpriteAnim.Play("HammerAttack");
        playerController.SpriteRenderer.color = Color.red;
    }

    private void ExecuteSpearSkill()
    {
        playerController.SpriteAnim.Play("SpearAttack");
        playerController.SpriteRenderer.color = Color.red;
    }

    private void ExecuteGauntletSkill()
    {
        playerController.SpriteAnim.Play("GauntletAttack");
        playerController.SpriteRenderer.color = Color.red;
    }


    private IEnumerator SkillCooldown( PlayerSkill _skillType)
    {
        //データからクールタイムを取得
        float coolTime = skillData.dicSkill.ContainsKey(_skillType) ? skillData.dicSkill[_skillType].coolTime : 0f;

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

    //public BlockDetector BlockDetector
    //{
    //    get => this.blockDetector;
    //}

    public DashHandler DashHandler
    {
        get => this.dash;
    }


    public bool IsDashing
    {
        get => this.dash.IsDashing;
        set { this.dash.IsDashing = value; }
    }

    public Vector3 DashDirec
    {
        get => this.dash.Direc;
        // set { this.dashDirec = value; }
    }
}


/// <summary>
/// 一定距離を移動
/// </summary>
/// <remarks>
///  fixUpdateで使う
/// </remarks>
//private void Move(float _speed, Vector3 _direc)
//{
//    playerController.RigidBody.MovePosition(playerController.RigidBody.position + _direc.normalized * _speed * Time.fixedDeltaTime);
//}

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


