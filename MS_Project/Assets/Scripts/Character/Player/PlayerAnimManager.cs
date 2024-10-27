using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのアニメーションイベントを管理するビヘイビア
/// </summary>
public class PlayerAnimManager : MonoBehaviour
{
    //PlayerControllerの参照
    PlayerController playerController;

    public float startTime;

    public float testTime;

    //アニメーション終了したか
    private bool isAnimEnd = false;

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;
    }

    /// <summary>
    /// ステート遷移時のリセット処理
    /// </summary>
    public void Reset()
    {
        isAnimEnd = false;
    }

    /// <summary>
    /// アニメーション終了フラグ設定
    /// </summary>
    void EndAnim()
    {
        isAnimEnd = true;
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
    void EnableCombo()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;
        skillManager.CanCombo=true;
    }


    /// <summary>
    /// 攻撃可能設定
    /// </summary>
    void EnableHit()
    {
        AttackColliderManager attackCollider = playerController.AttackCollider;

        attackCollider.SetHit(true);
    }

    /// <summary>
    /// 攻撃不可設定
    /// </summary>
    void DisableHit()
    {
        AttackColliderManager attackCollider = playerController.AttackCollider;

        attackCollider.SetHit(false);
    }

    /// <summary>
    /// 突進開始
    /// </summary>
    void StartDash()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;

        skillManager.DashHandler.StartDash(true);

        startTime = playerController.SpriteAnim.GetCurrentAnimatorStateInfo(0).normalizedTime*
             playerController.SpriteAnim.GetCurrentAnimatorStateInfo(0).length;

        testTime = playerController.SpriteAnim.GetCurrentAnimatorStateInfo(0).length - startTime;


    }

    /// <summary>
    /// 突進終了
    /// </summary>
    void EndDash()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;

        skillManager.DashHandler.EndDash();
    }

    public bool IsAnimEnd
    {
        get => this.isAnimEnd;
        // set { this.dashDirec = value; }
    }
}
