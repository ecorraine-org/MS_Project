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
    ///攻撃可能設定
    /// </summary>
    //void SetHit(bool _canAttack)
    //{
    //    playerController.AttackCollider.CanHit = _canAttack;

    //}
}
