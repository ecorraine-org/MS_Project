using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraStateBase : MonoBehaviour
{
    protected CameraStateContext _context;
    protected CameraSettings _settings;

    public virtual void Init(CameraStateContext context)
    {
        _context = context;
        _settings = context.settings;
    }

    public virtual void OnStart()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnLateUpdate()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnEnd()
    {
    }
}
