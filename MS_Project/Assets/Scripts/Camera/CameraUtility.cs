using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラユーティリティ
/// </summary>
public class CameraUtility
{
    /// <summary>
    /// カメラの向きを取得
    /// </summary>
    public static Vector3 SmoothDampPosition(
        Vector3 current,
        Vector3 target,
        ref Vector3 currentVelocity,
        float smoothTime,
        float maxSpeed,
        float deltaTime)
    {
        return Vector3.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    }

    /// <summary>
    /// カメラの回転を取得
    /// </summary>
    public static Quaternion SmoothDampRotation(
        Quaternion current,
        Quaternion target,
        ref Quaternion currentVelocity,
        float smoothTime,
        float maxSpeed,
        float deltaTime)
    {
        return Quaternion.Slerp(current, target, smoothTime);
    }
}
