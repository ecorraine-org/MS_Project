using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraLockOnSettings
{
    public float MaxLockOnDistance = 10f;    // ロックオンの最大距離
    public float TransitionSpeed = 5f;       // ロックオンの移行速度
    public float PlayerWeight = 0.5f;        // プレイヤーの重み
    public float TargetWeight = 0.5f;        // ターゲットの重み
    public LayerMask LockOnLayer;            // ロックオンするレイヤー
    public Vector3 OffsetFromTarget = new Vector3(0, 1.5f, 0); // ターゲットからのオフセット
    public bool UseScreenSpaceTargeting = true; // スクリーンスペースでのターゲット指定
    public float CameraHeight = 1.5f;        // カメラの高さ
    public float CameraDistance = 3f;        // カメラの距離
    [Header("Cinemachine Settings")]
    public float DeadZoneWidth = 0.1f;       // デッドゾーンの幅
    public float DeadZoneHeight = 0.1f;      // デッドゾーンの高さ
    public float SoftZoneWidth = 0.1f;       // ソフトゾーンの幅
    public float SoftZoneHeight = 0.1f;      // ソフトゾーンの高さ
}
