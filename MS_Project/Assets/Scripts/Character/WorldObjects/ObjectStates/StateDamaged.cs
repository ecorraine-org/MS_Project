using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDamaged : ObjectState
{
    //PlayerController player;

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        //player = GameObject.Find("Player").GetComponent<PlayerController>();

        if (objController.Type == WorldObjectType.StaticObject)
        {
            staticObj.GenerateOnomatopoeia(objController.gameObject, staticObj.ObjectStatus.StatusData.onomatoData);
        }
        else if (objController.Type == WorldObjectType.Enemy && enemy != null)
        {
            enemy.Anim.Play("Damaged", 0, 0.0f);

            int playerMode = ((int)player.ModeManager.Mode);
            OnomatopoeiaData attackOnomatoData = player.StatusManager.StatusData.onomatoAttackData[playerMode];
            enemy.GenerateOnomatopoeia(enemy.gameObject, attackOnomatoData);
        }
    }

    public override void Tick()
    {
        if (objStateHandler.CheckDeath()) return;

        // ダメージ(連撃)チェック
        if (objStateHandler.CheckHit()) return;

        if (objController.Type == WorldObjectType.StaticObject)
        {
            objController.State.TransitionState(ObjectStateType.Idle);
        }
        else if (objController.Type == WorldObjectType.Enemy)
        {
            if (enemy.AnimManager != null && enemy.AnimManager.IsAnimEnd)
            {
                //Debug.Log("ダメージ");
                objController.State.TransitionState(ObjectStateType.Idle);
            }
        }
    }

    public override void FixedTick()
    {
    }

    public override void Exit()
    {
    }
}
