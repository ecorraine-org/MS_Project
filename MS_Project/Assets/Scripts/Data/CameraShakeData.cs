using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraShakeData", menuName = "ScriptableObjects/Camera/CameraShakeData", order = 0)]
public class CameraShakeData : ScriptableObject
{
    [Header("名前")]
    private string _name;

    [Header("開始時間")]
    private float _startTime;

    [Header("総時間")]
    private float _totalTime;

    [Header("減衰率")]
    private float _DecayRate;

    [Header("強さ")]
    private float _scale;

    [Header("アクティブ情報")]
    private bool _isActive;

    [Header("シェイクの強さ")]
    private float _shakeStrength;

    [Header("バイブレーションの強さ")]
    private float _vibrationStrength;

}
