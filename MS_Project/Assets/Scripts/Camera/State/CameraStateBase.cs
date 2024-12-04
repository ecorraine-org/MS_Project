using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraStateBase : ICameraState
{
    protected Transform CameraTransform { get; private set; }
    protected Transform TargetTransform { get; private set; }
    protected CameraEffectController EffectController { get; private set; }
    protected CameraSettings Settings { get; private set; }
    protected CameraStateManager StateManager { get; private set; }

    public virtual void EnterState(CameraStateContext context)
    {
        CameraTransform = context.cameraTransform;
        TargetTransform = context.targetTransform;
        EffectController = context.cameraEffectController;
        Settings = context.settings;

        OnStateEnter();
    }

    public virtual void UpdateState(CameraStateContext context)
    {
        OnStateUpdate();
    }

    public virtual void ExitState(CameraStateContext context)
    {
        OnStateExit();
    }

    protected virtual void OnStateEnter() { }
    protected virtual void OnStateUpdate() { }
    protected virtual void OnStateExit() { }
}
