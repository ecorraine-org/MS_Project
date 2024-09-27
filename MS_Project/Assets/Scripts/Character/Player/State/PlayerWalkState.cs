using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerState
{


    //���͕���
    UnityEngine.Vector2 inputDirec;

    public override void Init(PlayerController _playerController)
    {
        base.Init(_playerController);




        // inputManager.BindAction(inputManager.InputControls.GamePlay.Walk, ExecuteWalk);
    }


    public override void Tick()
    {
        //�_���[�W�`�F�b�N
        playerController.StateManager.CheckHit();

        //�����擾
        inputDirec = inputManager.GetMoveDirec();

        //�A�C�h���֑J��
        if (inputDirec.magnitude <= 0)
        {
            playerController.StateManager.TransitionState(StateType.Idle);
            return;
        }

        //�����ݒ�
        playerController.SetEightDirection();

        //�A�j���[�V�����ݒ�
        playerController.SetWalkAnimation();

    }

    public override void FixedTick()
    {
        //�ړ����x�擾
        PlayerStatusManager PlayerStatusManager = playerController.StatusManager;
        float moveSpeed = PlayerStatusManager.StatusData.velocity;

        rb.velocity = new UnityEngine.Vector3(inputDirec.x * moveSpeed, rb.velocity.y, inputDirec.y * moveSpeed);
    }

    public override void Exit()
    {
        //inputManager.UnBindAction(inputManager.InputControls.GamePlay.Walk);
    }
}
