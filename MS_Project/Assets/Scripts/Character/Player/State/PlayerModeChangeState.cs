using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModeChangeState : PlayerState
{
    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(false);

        base.Init(_playerController);
        Debug.Log("切替ステート");

        //モードチェンジ処理
        playerModeManager.Mode = PlayerMode.Sword;
    }

    public override void Tick()
    {
        //アイドルへ遷移
        playerController.StateManager.TransitionState(StateType.Idle);

    }

    public override void FixedTick()
    {

    }

    public override void Exit()
    {

    }
}
