using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerState
{
    public override void Init(PlayerController _playerController)
    {
        base.Init(_playerController);
        Debug.Log("�X�L���X�e�[�g");
    }

    public override void Tick()
    {
        playerController.StateManager.TransitionState(StateType.Idle);
    }

    public override void FixedTick()
    {

    }

    public override void Exit()
    {

    }
}
