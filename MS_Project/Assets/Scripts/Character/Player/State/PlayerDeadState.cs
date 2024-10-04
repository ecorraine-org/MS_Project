using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public override void Init(PlayerController _playerController)
    {
        base.Init(_playerController);
        Debug.Log("���S�X�e�[�g");
    }

    public override void Tick()
    {
        Debug.Log("Player is dead");
    }

    public override void FixedTick()
    {

    }

    public override void Exit()
    {

    }
}
