using UnityEngine;

public class MiniMapOverall : MonoBehaviour
{
    [SerializeField] private Transform player; // プレイヤーのTransformを設定
    [SerializeField] private Vector3 offset = new Vector3(0, 50, 0); // ミニマップカメラの位置調整

    private void LateUpdate()
    {
        if (player != null)
        {
            // プレイヤーの位置にオフセットを加えてカメラを移動
            transform.position = player.position + offset;
        }
    }
}
