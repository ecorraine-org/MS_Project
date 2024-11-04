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
    /// 連撃フレーム
    /// </summary>
    public override void EnableCombo()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;
        skillManager.CanCombo=true;
    }


    /// <summary>
    /// 攻撃可能設定
    /// </summary>
    public override void EnableHit()
    {
        AttackColliderManager attackCollider = playerController.AttackCollider;

        attackCollider.SetHit(true);
    }

    /// <summary>
    /// 攻撃不可設定
    /// </summary>
    public override void DisableHit()
    {
        AttackColliderManager attackCollider = playerController.AttackCollider;

        attackCollider.SetHit(false);
    }

    /// <summary>
    /// 突進開始
    /// </summary>
    public override void StartDash()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;

        //方向、画像反転設定
        playerController.SetEightDirection();
        skillManager.DashHandler.Begin(true, playerController.CurDirecVector);

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
