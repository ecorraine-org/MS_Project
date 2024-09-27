using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerState
{


    //入力方向
    UnityEngine.Vector2 inputDirec;

    public override void Init(PlayerController _playerController)
    {
        base.Init(_playerController);


    }

    public override void Tick()
    {
        //ダメージチェック
        playerController.StateManager.CheckHit();

        //攻撃へ遷移
        bool isAttack = inputManager.GetAttackTrigger();
        if (isAttack) playerController.StateManager.TransitionState(StateType.Attack);

        //方向取得
        inputDirec = inputManager.GetMoveDirec();

        //アイドルへ遷移
        if (inputDirec.magnitude <= 0)
        {
            playerController.StateManager.TransitionState(StateType.Idle);
            return;
        }

        //方向設定
        playerController.SetEightDirection();

        //アニメーション設定
        playerController.SetWalkAnimation();

    }

    public override void FixedTick()
    {
        //移動速度取得
        PlayerStatusManager PlayerStatusManager = playerController.StatusManager;
        float moveSpeed = PlayerStatusManager.StatusData.velocity;

        rb.velocity = new UnityEngine.Vector3(inputDirec.x * moveSpeed, rb.velocity.y, inputDirec.y * moveSpeed);
    }

    public override void Exit()
    {

    }
}
