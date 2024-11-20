using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDamaged : ObjectState
{
   // PlayerController player;

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

     //   player = GameObject.Find("Player").GetComponent<PlayerController>();

        if (enemy != null)
        {
            enemy.Anim.Play("Damaged",0,0.0f);

            int playerMode = ((int)player.ModeManager.Mode);
            OnomatopoeiaData attackOnomatoData = player.StatusManager.StatusData.onomatoAttackData[playerMode];
            enemy.GenerateOnomatopoeia(attackOnomatoData);
        }
    }

    public override void Tick()
    {
        // ダメージ(連撃)チェック 
        if (objStateHandler.CheckHit()) return;

        if (enemy.AnimManager != null && enemy.AnimManager.IsAnimEnd)
        {
            //    Debug.Log("ダメージ");
            objController.State.TransitionState(ObjectStateType.Idle);
        }
           
        if (enemy.AnimManager == null)
            objController.State.TransitionState(ObjectStateType.Idle);
    }

    public override void FixedTick()
    {
    }

    public override void Exit()
    {
    }
}
