using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraCollisionSettings
{
    public LayerMask CollisionLayers;   // カメラの衝突判定に使用するレイヤーマスク
    public float CollisionRadius = 0.2f;    // カメラの衝突判定の半径
    public float MinDistance = 1f;      // カメラとプレイヤーの最小距離
    public float SmoothTime = 0.1f;     // カメラの移動のスムージング
    public bool UseScreenSpaceCorrection = true;    // 画面スペースの補正を使用するか
    public float CorrectionStrength = 1f;       // 補正の強さ
}
