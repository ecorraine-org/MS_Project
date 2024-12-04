using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラの基本設定
/// </summary>
[CreateAssetMenu(fileName = "CameraSettings", menuName = "ScriptableObjects/Camera/Settings")]
public class CameraSettings : ScriptableObject
{
    public CameraBasicSettings BasicSettings;
    public CameraEffectSettings EffectSettings;
    public CameraCollisionSettings CollisionSettings;
    public CameraFollowSettings FollowSettings;
    public CameraLockOnSettings LockOnSettings;
}