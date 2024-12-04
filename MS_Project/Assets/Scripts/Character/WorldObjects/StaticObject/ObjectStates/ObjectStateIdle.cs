using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateIdle : ObjectState
{
    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);
    }

    public override void Tick()
    {
        base.Tick();

        //破壊チェック
        if (objectStateHandler.CheckDeath()) return;

        //ダメージチェック
        if (objectStateHandler.CheckHit()) return;


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
