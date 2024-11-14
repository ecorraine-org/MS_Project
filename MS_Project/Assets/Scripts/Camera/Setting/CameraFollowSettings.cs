using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraFollowSettings
{
    public float PositionSmoothTime = 0.1f; // カメラの移動のスムージング
    public float RotationSmoothTime = 0.05f;    // カメラの回転のスムージング
    public Vector3 PositionOffset = new Vector3(0, 2, -5);  // カメラの位置のオフセット
    public Vector3 LookAtOffset = new Vector3(0, 1, 0); // カメラの注視点のオフセット
    public bool LockX;                                  // X軸をロックするか
    public bool LockY;                                  // Y軸をロックするか
    public bool LockZ;                                  // Z軸をロックするか
    public Vector2 HeightMinMax = new Vector2(-1f, 5f); // カメラの高さの最小値と最大値
    public Vector2 DistanceMinMax = new Vector2(3f, 10f);    // カメラとプレイヤーの距離の最小値と最大値
}