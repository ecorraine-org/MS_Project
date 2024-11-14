using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDamaged : ObjectState
{
    PlayerController player;

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        player = GameObject.Find("Player").GetComponent<PlayerController>();

        if (enemy != null)
        {
            enemy.Anim.Play("Damaged");

            int playerMode = ((int)player.ModeManager.Mode);
            OnomatopoeiaData attackOnomatoData = player.StatusManager.StatusData.onomatoAttackData[playerMode];
            enemy.GenerateOnomatopoeia(attackOnomatoData);
        }
    }

    public override void Tick()
    {
        objController.State.TransitionState(ObjectStateType.Idle);
    }

    public override void FixedTick()
    {
    }

    public override void Exit()
    {
    }
}
