using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    //入力方向
    UnityEngine.Vector2 inputDirec;

    public override void Init(PlayerController _playerController)
    {
        base.Init(_playerController);

        //   inputManager.BindWalk(ExecuteWalk);
        //  inputManager.BindAction(inputManager.InputControls.GamePlay.Walk, ExecuteWalk);

    }

    public override void Tick()
    {
        //ダメージチェック
        playerController.StateManager.CheckHit();

        //アニメーション設定
        playerController.SetWalkAnimation();

        inputDirec = inputManager.GetMoveDirec();

        //移動へ遷移
        if (inputDirec.magnitude > 0)
            playerController.StateManager.TransitionState(StateType.Walk);

    }

    public override void FixedTick()
    {



    }

    public override void Exit()
    {
        //   inputManager.UnBindWalk(ExecuteWalk);
        //  inputManager.UnBindWalk();

        // inputManager.UnBindAction(inputManager.InputControls.GamePlay.Walk);
    }



}
