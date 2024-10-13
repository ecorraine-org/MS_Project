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

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;
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

       // attackCollider.SetHit(true);

    }

    /// <summary>
    /// 突進終了
    /// </summary>
    void EndDash()
    {
        PlayerSkillManager skillManager = playerController.SkillManager;

       // attackCollider.SetHit(false);
    }
}
