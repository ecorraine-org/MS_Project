using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤースキル管理のビヘイビア
/// </summary>
public class PlayerSkillManager : MonoBehaviour
{
    // PlayerControllerの参照
    PlayerController playerController;

    BattleManager battleManager;

    [SerializeField, Header("突進処理ビヘイビア")]
    DashHandler dash;

    [SerializeField, Header("ステータスデータ")]
    PlayerSkillData skillData;

    [SerializeField, Header("ヒットリアクションデータ")]
    PlayerHitData playerHitData;

    //*********
    //ノーマル攻撃
    [SerializeField, NonEditable, Header("攻撃段階")]
    int attackStage = 0;

    //最大攻撃段階数
    int maxAttackStage = 0;

    [SerializeField, NonEditable, Header("ノーマル攻撃の攻撃力")]
    float attackDamage;
    //*********

    [SerializeField, Header("今のスキルのクールタイム")]
    float curSkillCoolTime;

    // 攻撃をキャンセルし、コンボできるかどうか
    [SerializeField, Header("キャンセルできるか")]
    bool canComboCancel = false;

    // コンボ入力できるかどうか
    [SerializeField, Header("コンボできるか")]
    bool canComboInput = false;

    // 当たり判定可能かどうか
    bool canHit = false;

    // 長押ししているか
    bool canCharge = false;

    [SerializeField, Header("弾発射装置")]
    BulletLauncher bulletLauncher;



    // 辞書<キー：スキル種類、値：クールタイム>
    private Dictionary<PlayerSkill, float> coolTimers = new Dictionary<PlayerSkill, float>();

    private Dictionary<PlayerSkill, bool> dicIsCharge = new Dictionary<PlayerSkill, bool>();

    private void Awake()
    {

        foreach (PlayerSkill skill in Enum.GetValues(typeof(PlayerSkill)))
        {
            coolTimers.Add(skill, 0f);
            dicIsCharge.Add(skill, false);
        }
    }

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;

        dash.Init(playerController);

        battleManager = playerController.BattleManager;
    }

    /// <summary>
    /// スキル発動するためのhp消費
    /// </summary>
    public bool HpCost(PlayerSkill _skillType)
    {
        // スキルのHPコストが現在のHPより少ないか
        if (skillData.dicSkill[_skillType].hpCost < playerController.StatusManager.CurrentHealth)
        {
            var life = playerController.GetComponentInChildren<ILife>();
            if (life != null)
            {
                // hp消費
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
        //  Debug.Log($"{_skillType}クールタイム中");//test
        if (coolTimers.ContainsKey(_skillType) && coolTimers[_skillType] <= 0f)
        {
            // Debug.Log($"{_skillType}発動");//test

            // スキル発動
            ExecuteSkill(_skillType);

            // クールタイム処理
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

    public void ExecuteSkillCharge(PlayerSkill _skillType)
    {
        dicIsCharge[_skillType] = true;

        switch (_skillType)
        {
            case PlayerSkill.Hammer:

                ExecuteHammerSkillCharge();
                break;

            default:
                break;
        }
    }

    public void ExecuteSkillChargeFinished(PlayerSkill _skillType)
    {
        dicIsCharge[_skillType] = false;

        switch (_skillType)
        {
            case PlayerSkill.Hammer:

                ExecuteHammerSkillChargeFinished();
                break;

            default:
                break;
        }
    }

    // キャンセルされた時のリセット処理
    public void Reset()
    {
        if (dash.IsDashing) dash.End();

        canComboCancel = false;

        canComboInput = false;

        canCharge = false;
    }

    public void ExecuteDodge(bool _canThrough, Vector3 _direc = default)
    {
        switch (playerController.CurrentDirec)
        {
            case Direction.Up:
                playerController.SpriteAnim.Play("WalkUp", 0, 0f);
                break;
            default:
                playerController.SpriteAnim.Play("Dash", 0, 0f);
                break;
        }

        // playerController.SpriteAnim.Play("Dash", 0, 0f);

        // 突進初期化
        dash.Speed = skillData.dicSkill[PlayerSkill.Dodge].dashSpeed;
        dash.Duration = skillData.dicSkill[PlayerSkill.Dodge].dashDuration;
        dash.CanThrough = true;

        // 向きによるアニメーション設定(反転するかどうか)
        //playerController.SetEightDirection();
        dash.Begin(_canThrough, _direc);
    }

    public void ExecuteEat()
    {
        playerController.SpriteAnim.Play("Eat");

        battleManager.slowSpeed = skillData.dicSkill[PlayerSkill.Eat].slowSpeed;
        battleManager.stopDuration = skillData.dicSkill[PlayerSkill.Eat].stopDuration;

    }

    private void ExecuteSwordSkill()
    {
        playerController.SpriteAnim.Play("SwordSkill");

        //ヒットストップ
        battleManager.slowSpeed = skillData.dicSkill[PlayerSkill.Sword].slowSpeed;
        battleManager.stopDuration = skillData.dicSkill[PlayerSkill.Sword].stopDuration;


        // 突進初期化
        dash.Speed = skillData.dicSkill[PlayerSkill.Sword].dashSpeed;
        dash.Duration = skillData.dicSkill[PlayerSkill.Sword].dashDuration;
        dash.CanThrough = false;
    }

    /// <summary>
    /// 真空連斬
    /// </summary>
    public void LaunchWindBlade()
    {
        bulletLauncher.SpriteFire(playerController.SpriteRenderer);
    }

    public void ExecuteHammerSkill()
    {
        playerController.SpriteAnim.Play("HammerSkill");

        //ヒットストップ
        battleManager.slowSpeed = skillData.dicSkill[PlayerSkill.Hammer].slowSpeed;
        battleManager.stopDuration = skillData.dicSkill[PlayerSkill.Hammer].stopDuration;


        dash.Speed = skillData.dicSkill[PlayerSkill.Hammer].dashSpeed;
        dash.Duration = skillData.dicSkill[PlayerSkill.Hammer].dashDuration;
        dash.CanThrough = false;
    }

    public void ExecuteHammerSkillCharge()
    {
        //playerController.SpriteAnim.Play("HammerSkill");

        //dash.Speed = skillData.dicSkill[PlayerSkill.Hammer].dashSpeed;
        //dash.Duration = skillData.dicSkill[PlayerSkill.Hammer].dashDuration;
    }

    public void ExecuteHammerSkillChargeFinished()
    {


        playerController.SpriteAnim.Play("HammerSkill2", 0, 0f);

        dash.Speed = skillData.dicSkill[PlayerSkill.Hammer].dashSpeed;
        dash.Duration = skillData.dicSkill[PlayerSkill.Hammer].dashDuration;
    }


    private void ExecuteSpearSkill()
    {
        playerController.SpriteAnim.Play("SpearSkill");
        playerController.SpriteRenderer.color = Color.red;

        //ヒットストップ
        battleManager.slowSpeed = skillData.dicSkill[PlayerSkill.Spear].slowSpeed;
        battleManager.stopDuration = skillData.dicSkill[PlayerSkill.Spear].stopDuration;


        // 突進初期化
        dash.Speed = skillData.dicSkill[PlayerSkill.Spear].dashSpeed;
        dash.Duration = skillData.dicSkill[PlayerSkill.Spear].dashDuration;
        dash.CanThrough = true;
    }

    private void ExecuteGauntletSkill()
    {
        playerController.SpriteAnim.Play("GauntletSkill");

        //ヒットストップ
        battleManager.slowSpeed = skillData.dicSkill[PlayerSkill.Gauntlet].slowSpeed;
        battleManager.stopDuration = skillData.dicSkill[PlayerSkill.Gauntlet].stopDuration;


        dash.Speed = skillData.dicSkill[PlayerSkill.Gauntlet].dashSpeed;
        dash.Duration = skillData.dicSkill[PlayerSkill.Gauntlet].dashDuration;
        dash.CanThrough = false;
    }

    #region swordAttack
    public void SwordAttackInit()
    {
        attackDamage = playerController.StatusManager.StatusData.swordAtk;
        maxAttackStage = 1;

        // 突進初期化
        dash.Speed = playerHitData.dicHitReac[playerController.ModeManager.Mode].moveSpeed;
        dash.Duration = -1;

        if (attackStage == 0)
        {
            playerController.SpriteAnim.Play("Attack", 0, 0f);
        }

        if (attackStage == 1)
        {
            Debug.Log("SwordAttack2");
            playerController.SpriteAnim.Play("SwordAttack2", 0, 0f);
        }
    }

    public void SwordNextAttack()
    {
        if (attackStage < maxAttackStage) attackStage++;
        else attackStage = 0;

        playerController.StateManager.TransitionState(StateType.Attack);
    }
    #endregion

    #region HammerAttack
    public void HammerAttackInit()
    {
        attackDamage = playerController.StatusManager.StatusData.hammerAtk;
        playerController.SpriteAnim.Play("HammerAttack", 0, 0f);

        // 突進初期化
        dash.Speed = playerHitData.dicHitReac[playerController.ModeManager.Mode].moveSpeed;
        dash.Duration = -1;
    }
    #endregion

    public void SpearAttackInit()
    {
        attackDamage = playerController.StatusManager.StatusData.spearAtk;
        playerController.SpriteAnim.Play("SpearAttack", 0, 0f);
    }

  
    public void GauntletAttackInit()
    {
        attackDamage = playerController.StatusManager.StatusData.gauntletAtk;
        playerController.SpriteAnim.Play("GauntletAttack", 0, 0f);
    }

    private IEnumerator SkillCooldown(PlayerSkill _skillType)
    {
        // データからクールタイムを取得
        float coolTime = skillData.dicSkill.ContainsKey(_skillType) ? skillData.dicSkill[_skillType].coolTime : 0f;

        // クールタイム設定
        coolTimers[_skillType] = coolTime;

        while (coolTimers[_skillType] > 0f)
        {
            coolTimers[_skillType] -= Time.deltaTime;

            // クールタイムを0以上にする
            coolTimers[_skillType] = Mathf.Max(coolTimers[_skillType], 0f);

            // スキルクールタイムを記録
            curSkillCoolTime = coolTimers[_skillType];

            yield return null;
        }

        Debug.Log($"{_skillType} スキルクールダウン完了");
    }

    public IReadOnlyDictionary<PlayerSkill, float> CoolTimers
    {
        get => coolTimers;
    }

    public Dictionary<PlayerSkill, bool> DicIsCharge
    {
        get => dicIsCharge;
        set { this.dicIsCharge = value; }
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

    public int AttackStage
    {
        get => attackStage;
        set { attackStage = value; }
    }

    public float AttackDamage
    {
        get => attackDamage;
        set { attackDamage = value; }
    }

    public Vector3 DashDirec
    {
        get => this.dash.Direc;
        // set { this.dashDirec = value; }
    }

    public PlayerSkillData SkillData
    {
        get => this.skillData;
        // set { this.dashDirec = value; }
    }

    public bool CanComboCancel
    {
        get => this.canComboCancel;
        set { this.canComboCancel = value; }
    }

    public bool CanCharge
    {
        get => this.canCharge;
        set { this.canCharge = value; }
    }

    public bool CanComboInput
    {
        get => this.canComboInput;
        set { this.canComboInput = value; }
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


