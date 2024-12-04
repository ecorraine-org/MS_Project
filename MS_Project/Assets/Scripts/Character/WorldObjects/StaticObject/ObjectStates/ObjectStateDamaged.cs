using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateDamaged : ObjectState
{
    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        staticObj.GenerateOnomatopoeia(staticObj.gameObject, objectStatusHandler.StatusData.onomatoData);
    }

    public override void Tick()
    {
        base.Tick();

        //破壊チェック
        if (objectStateHandler.CheckDeath()) return;

        //ダメージ(連撃)チェック
        if (objectStateHandler.CheckHit()) return;

        objectStateHandler.TransitionState(ObjectStateType.Idle);
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
