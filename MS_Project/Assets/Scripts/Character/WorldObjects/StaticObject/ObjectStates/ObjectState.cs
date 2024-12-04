using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectState : WorldObjectState
{
    protected ObjectController staticObj;
    protected ObjectStatusHandler objectStatusHandler;
    protected ObjectStateHandler objectStateHandler;

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        if (objController.Type != WorldObjectType.StaticObject)
        {
            CustomLogger.Log("種類違いのステートが検出されました\n" + objController.name + "：" + objController.Type.ToString());
            return;
        }

        staticObj = objController as ObjectController;
        if (staticObj == null)
        {
            CustomLogger.Log(objController.name + "が取得できませんでした");
            return;
        }

        objectStatusHandler = staticObj.Status;
        objectStateHandler = staticObj.State;

        player = staticObj.PlayerController;
    }

    public override void Tick()
    {
        if (staticObj == null)
            return;
    }

    public override void FixedTick()
    {
        if (staticObj == null)
            return;
    }

    public override void Exit()
    {
        if (staticObj == null)
            return;
    }
}
