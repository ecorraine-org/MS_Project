using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraEffectData
{
    public CameraEffectType EffectType; // エフェクトの種類
    public float Intensity;             // エフェクトの強さ
    public float Duration;              // エフェクトの持続時間
    public Vector3 Direction;           // エフェクトの方向
    public AnimationCurve Curve;        // エフェクトの変化
    public bool UseRandomDirection;             // ランダムな方向にエフェクトするか

    public static CameraEffectData CreateShake(float intensity, float duration)
    {
        return new CameraEffectData
        {
            EffectType = CameraEffectType.Shake,
            Intensity = intensity,
            Duration = duration,
            Curve = AnimationCurve.EaseInOut(0, 1, 1, 0)
        };
    }
}