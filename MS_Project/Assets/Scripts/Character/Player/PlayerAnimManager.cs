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

    void Attack()
    {
        // playerController.Attack();

    }
}
