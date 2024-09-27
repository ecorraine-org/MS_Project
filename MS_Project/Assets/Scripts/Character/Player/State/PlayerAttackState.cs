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
        //ダメージチェック
        playerController.StateManager.CheckHit();
    }

    public override void FixedTick()
    {

    }

    public override void Exit()
    {

    }
}
