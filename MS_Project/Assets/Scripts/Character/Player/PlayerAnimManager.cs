using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    //PlayerController�̎Q��
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
