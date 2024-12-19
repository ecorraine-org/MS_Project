using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerState
{


    //入力方向
    UnityEngine.Vector2 inputDirec;

    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(false);

        base.Init(_playerController);
    }

    public override void Tick()
    {
        //ダメージチェック
        if (playerController.StateManager.CheckHit()) return;

        //攻撃へ遷移
        if (playerStateManager.CheckAttack()) return;

        //捕食へ遷移
        if (playerStateManager.CheckEat()) return;

        //スキルへ遷移
        if (playerStateManager.CheckSkill()) return;

        //回避へ遷移
        if (playerStateManager.CheckDodge()) return;

        //方向設定
        playerController.SetEightDirection();

        //アニメーション設定
        playerController.SetWalkAnimation();

        //方向取得
        inputDirec = inputManager.GetMoveDirec();

        //アイドルへ遷移
        if (inputDirec.magnitude <= 0)
        {
            playerController.StateManager.TransitionState(StateType.Idle);
            return;
        }


    }

    public override void FixedTick()
    {
        //移動速度取得
        float moveSpeed = statusManager.StatusData.velocity * buffManager.BuffEffect.speedUpRate;

        rb.velocity = new UnityEngine.Vector3(inputDirec.x * moveSpeed, rb.velocity.y, inputDirec.y * moveSpeed);
    }

    public override void Exit()
    {

    }
}
