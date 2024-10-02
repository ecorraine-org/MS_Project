using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEatState : PlayerState
{
    public override void Init(PlayerController _playerController)
    {
        base.Init(_playerController);
        Debug.Log("捕食ステート");
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
