using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラエフェクトインターフェース
/// </summary>
public interface ICameraEffect
{
    void StartEffect(CameraEffectData cameraEffectData);
    void UpdateEffect();
    void EndEffect();
}
