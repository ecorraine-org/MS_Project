using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerState
{
    [SerializeField, Header("硬直時間")]
    float hitStunTime = 0.5f;

    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(false);

        base.Init(_playerController);
        Debug.Log("被撃ステート");

        spriteAnim.Play("Damaged", 0, 0f);
        playerController.SpriteRenderer.color = Color.red;

        TimerUtility.TimeBasedTimer(this, hitStunTime,()=> playerController.StateManager.TransitionState(StateType.Idle));
    }

    public override void Tick()
    {

     //   playerController.StateManager.TransitionState(StateType.Idle);
    }

    public override void FixedTick()
    {

    }

    public override void Exit()
    {
        playerController.SpriteRenderer.color = Color.white;
    }
}
