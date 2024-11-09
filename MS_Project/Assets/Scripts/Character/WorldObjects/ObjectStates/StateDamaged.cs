using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDamaged : ObjectState
{
    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);
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
