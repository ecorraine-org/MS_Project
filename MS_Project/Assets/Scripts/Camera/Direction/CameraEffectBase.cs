using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラエフェクト基底クラス
/// </summary>
public abstract class CameraEffectBase : ICameraEffect
{
    protected CameraEffectData cameraEffectData { get; private set; } // エフェクトデータ
    protected float ElapsedTime { get; private set; } // 経過時間
    protected bool isActive { get; private set; }   // アクティブ状態
    public bool isPlaying => isActive;  // 再生中か
    public event System.Action<CameraEffectType> OnEffectCompleted;  // エフェクト完了時のイベント

    public virtual void Play(CameraEffectData data)
    {
        cameraEffectData = data;
        ElapsedTime = 0;
        isActive = true;
        OnEffectStart();
    }

    public virtual void Stop()
    {
        isActive = false;
        OnEffectStop();
    }

    public virtual void Update()
    {
        if (!isActive) return;

        ElapsedTime += Time.deltaTime;
        if (ElapsedTime >= cameraEffectData.Duration)
        {
            Stop();
            return;
        }

        OnEffectUpdate();
    }


    protected virtual void OnEffectStart() { }
    protected virtual void OnEffectUpdate() { }
    protected virtual void OnEffectStop() { }

    protected float GetNormalizedTime()
    {
        return Mathf.Clamp01(ElapsedTime / cameraEffectData.Duration);
    }
}
