using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraShakeData
{
    private string Id;   // シェイクのID
    private float Intensity; // シェイクの強さ
    private float StartTime; // シェイクの開始時間
    private float Duration;  // シェイクの持続時間
    private Vector3 Direction;   // シェイクの方向
    private AnimationCurve IntensityCurve;   // シェイクの強さの変化
    private bool UseRandomDirection; // ランダムな方向にシェイクするか
}