using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public override void Init(PlayerController _playerController)
    {
        base.Init(_playerController);

    }

    public override void Tick()
    {
        //�_���[�W�`�F�b�N
        playerController.StateManager.CheckHit();
    }

    public override void FixedTick()
    {

    }

    public override void Exit()
    {

    }
}
