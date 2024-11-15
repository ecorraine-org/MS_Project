using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラエフェクト基底クラス
/// </summary>
public class CameraEffectBase : ICameraEffect
{
    protected CameraEffectData cameraEffectData { get; private set; }
    protected float ElapsedTime { get; private set; }
    protected bool isActive { get; private set; }

    public CameraEffectBase(CameraEffectData cameraEffectData)
    {
        this.cameraEffectData = cameraEffectData;
    }

    public virtual void OnEnter()
    {
        isActive = true;
        ElapsedTime = 0;
    }

    public virtual void OnUpdate()
    {
        ElapsedTime += Time.deltaTime;
    }

    public virtual void OnExit()
    {
        isActive = false;
    }

    public void StartEffect(CameraEffectData cameraEffectData)
    {
        this.cameraEffectData = cameraEffectData;
        OnEnter();
    }

    public void UpdateEffect()
    {
        OnUpdate();
    }

    public void EndEffect()
    {
        OnExit();
    }
}
