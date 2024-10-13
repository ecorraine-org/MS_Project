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

    [SerializeField, Header("障害物検出ディテクター")]
    BlockDetector blockDetector;

    [SerializeField, Header("ステータスデータ")]
    PlayerSkillData skillData;

    [SerializeField, Header("今のスキルのクールタイム")]
    float curSkillCoolTime;

    [SerializeField, Header("ダッシュ持続時間")]
    private float dashDuration = 0.3f;

    [SerializeField, Header("ダッシュ速度")]
    private float dashSpeed = 4.0f;

    //ダッシュ方向
    UnityEngine.Vector3 dashDirec;

    //ダッシュ中かどうか
    private bool isDashing = false;

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

        blockDetector.Init(_playerController);

        blockDetector.Distance = dashSpeed * dashDuration;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            //敵と重ならないため
            //移動先に敵がいなければ、敵との当たり判定を無視する
            if (!blockDetector.IsColliding)
            {
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Onomatopoeia"), true);
            }

            blockDetector.IsEnabled = false;
            blockDetector.Distance = dashSpeed * dashDuration;

           // Move(dashSpeed, dashDirec);
           //一定距離を移動
            playerController.RigidBody.MovePosition(playerController.RigidBody.position + dashDirec.normalized * dashSpeed * Time.fixedDeltaTime);
        }
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

  
    

    public void Dash(Vector3 _direc)
    {
        Debug.Log("DASH!!!!");

        dashDirec = _direc;

        //入力をチェック
        if (dashDirec != UnityEngine.Vector3.zero)
        {
            //ダッシュ処理
            StartCoroutine(DashCoroutine());

            Debug.Log("DASH!!!!");
            isDashing = true;
   
        }
    }

    public IEnumerator DashCoroutine()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {

            yield return null;
        }


        EndDash();
    }

    private void EndDash()
    {
        blockDetector.IsEnabled = true;
        isDashing = false;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Onomatopoeia"), false);

        Debug.Log("DASH END!!!!");
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

    public BlockDetector BlockDetector
    {
        get => this.blockDetector;
    }

    public bool IsDashing
    {
        get => this.isDashing;
        set { this.isDashing = value; }
    }

    public Vector3 DashDirec
    {
        get => this.dashDirec;
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


