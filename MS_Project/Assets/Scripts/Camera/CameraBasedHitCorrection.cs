using UnityEngine;

/// <summary>
/// カメラベースの当たり判定補正を提供するクラス
/// </summary>
public class CameraBasedHitCorrection : MonoBehaviour
{
    [Header("補正の強さ(奥行き)")]
    [SerializeField] private float depthTolerance = 1f;
    [Header("デバッグ用のコライダーの色")]
    [SerializeField] private Color normalColliderColor = Color.yellow;
    [Header("デバッグ用の補正されたコライダーの色")]
    [SerializeField] private Color correctedColliderColor = Color.green;

    private Camera mainCamera;

    private void Awake() => mainCamera = Camera.main;

    /// <summary>
    /// カメラ視点での当たり判定補正を行う
    /// </summary>
    /// <param name="attackerPosition">攻撃者の位置</param>
    /// <param name="targetPosition">ターゲットの位置</param>
    /// <param name="attackSize">攻撃の大きさ</param>
    /// <returns>補正された当たり判定かどうか</returns>
    public bool IsHitCorrected(Vector3 attackerPosition, Vector3 targetPosition, Vector3 attackSize)
    {
        var attackerScreenPos = mainCamera.WorldToScreenPoint(attackerPosition);
        var targetScreenPos = mainCamera.WorldToScreenPoint(targetPosition);

        var screenDistance = Vector2.Distance(
            new Vector2(attackerScreenPos.x, attackerScreenPos.y),
            new Vector2(targetScreenPos.x, targetScreenPos.y)
        );

        var maxScreenSize = Mathf.Max(attackSize.x, attackSize.y) * mainCamera.pixelHeight / 2f;

        return screenDistance <= maxScreenSize &&
               Mathf.Abs(attackerScreenPos.z - targetScreenPos.z) <= depthTolerance;
    }

    /// <summary>
    /// デバッグ用にコライダーを可視化する
    /// </summary>
    /// <param name="position">コライダーの位置</param>
    /// <param name="size">コライダーのサイズ</param>
    /// <param name="isCorrected">補正されたかどうか</param>
    public void VisualizeCollider(Vector3 position, Vector3 size, bool isCorrected)
    {
        Gizmos.color = isCorrected ? correctedColliderColor : normalColliderColor;
        Gizmos.DrawWireCube(position, size);
    }
}