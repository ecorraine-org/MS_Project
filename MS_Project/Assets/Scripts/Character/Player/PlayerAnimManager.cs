using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのアニメーションイベントを管理するビヘイビア
/// </summary>
public class PlayerAnimManager : AnimManager
{
    //PlayerControllerの参照
    PlayerController playerController;

    public float startTime;

    public float testTime;

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;
    }

    /// <summary>
    /// 弾を発射、真空連斬
    /// </summary>
    void LaunchWindBlade()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;
        skillManager.LaunchWindBlade();
    }

    /// <summary>
    /// 連撃キャンセルフレーム
    /// </summary>
    public override void EnableCombo()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;
        skillManager.CanComboCancel = true;
        Debug.Log("EnableCombo");
    }

    /// <summary>
    /// 連撃キャンセルフレーム
    /// </summary>
    public void DisableCombo()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;
        skillManager.CanComboCancel = false;
    }

    /// <summary>
    /// コンボ受付フレーム
    /// </summary>
    public void EnableComboInput()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;
        skillManager.CanComboInput = true;
        Debug.Log("EnableComboInput");
    }


    /// <summary>
    /// 攻撃可能設定
    /// </summary>
    public override void EnableHit()
    {

        AttackColliderManagerV2 attackColliderV2 = playerController.AttackColliderV2;
        attackColliderV2.StartHit();

    }

    /// <summary>
    /// 攻撃不可設定
    /// </summary>
    public override void DisableHit()
    {
        AttackColliderManagerV2 attackColliderV2 = playerController.AttackColliderV2;
        attackColliderV2.EndHit();

    }

    /// <summary>
    /// 長押し
    /// </summary>
    public void StartCharge()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;
        skillManager.CanCharge=true;
    }

    /// <summary>
    /// 突進開始
    /// </summary>
    public override void StartDash()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;

        //方向、画像反転設定
        playerController.SetEightDirection();
       // skillManager.DashHandler.Begin(true, playerController.CurDirecVector);
        skillManager.DashHandler.Begin(playerController.GetForward());
        

        startTime = playerController.SpriteAnim.GetCurrentAnimatorStateInfo(0).normalizedTime*
             playerController.SpriteAnim.GetCurrentAnimatorStateInfo(0).length;

        testTime = playerController.SpriteAnim.GetCurrentAnimatorStateInfo(0).length - startTime;


    }

    /// <summary>
    /// 突進終了
    /// </summary>
    public override void EndDash()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;

        skillManager.DashHandler.End();
    }

  
}
