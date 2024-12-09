using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラエフェクトインターフェース
/// </summary>
public interface ICameraEffect
{
    bool isPlaying { get; }
    void Play(CameraEffectData cameraEffectData);
    void Update();
    void Stop();
    //event System.Action<CameraEffectType> OnEffectCompleted;
}
