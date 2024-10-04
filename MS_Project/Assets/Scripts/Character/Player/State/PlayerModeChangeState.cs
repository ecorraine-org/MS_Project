using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModeChangeState : PlayerState
{
    public override void Init(PlayerController _playerController)
    {
        base.Init(_playerController);
        Debug.Log("�ؑփX�e�[�g");

        //���[�h�`�F���W����
        playerController.Mode = PlayerMode.Sword;
    }

    public override void Tick()
    {
        //�A�C�h���֑J��
        playerController.StateManager.TransitionState(StateType.Idle);

    }

    public override void FixedTick()
    {

    }

    public override void Exit()
    {

    }
}
