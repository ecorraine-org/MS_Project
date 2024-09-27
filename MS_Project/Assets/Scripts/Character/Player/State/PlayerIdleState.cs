using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    //���͕���
    UnityEngine.Vector2 inputDirec;

    public override void Init(PlayerController _playerController)
    {
        base.Init(_playerController);

        // inputManager.BindAction(inputManager.InputControls.GamePlay.Attack, ExecuteAttack);

    }



    public override void Tick()
    {
        //�_���[�W�`�F�b�N
        playerController.StateManager.CheckHit();

        //�U���֑J��
        bool isAttack = inputManager.GetAttackTrigger();
        if (isAttack) playerController.StateManager.TransitionState(StateType.Attack);

        //�A�j���[�V�����ݒ�
        playerController.SetWalkAnimation();

        inputDirec = inputManager.GetMoveDirec();

        //�ړ��֑J��
        if (inputDirec.magnitude > 0)
            playerController.StateManager.TransitionState(StateType.Walk);

    }

    public override void FixedTick()
    {



    }

    public override void Exit()
    {

        // inputManager.UnBindAction(inputManager.InputControls.GamePlay.Attack);
    }



}
