using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraEffectSettings
{
    public float MaxShakeIntensity = 1f;    // シェイクの最大強度
    public float MaxShakeDuration = 2f; // シェイクの最大持続時間
    public float ShakeFalloff = 1f;     // シェイクの減衰率
    public AnimationCurve DefaultShakeCurve;    // シェイクの強さの変化

    public float MaxZoomFOV = 90f;      // ズームの最大FOV
    public float MinZoomFOV = 30f;      // ズームの最小FOV
    public float ZoomSpeed = 5f;        // ズームの速度

    public float MaxRollAngle = 30f;   // ロールの最大角度
    public float RollSpeed = 3f;        // ロールの速度
    public Color DefaultColor = Color.white;    // デフォルトの色

    public Dictionary<string, CameraShakeData> ShakePresets = new();    // シェイクのプリセット
}