using UnityEngine;

public class CameraBossEnemy : MonoBehaviour
{
    public Camera targetCamera;         // 対象のカメラ
    public string targetLayerName = ""; // 写したいレイヤー名

    void Start()
    {
        
        // レイヤーマスクを取得
        int layerMask = LayerMask.GetMask(targetLayerName);

        if (layerMask == 0)
        {
            //
            return;
        }

        // カメラのCulling Maskを設定
        targetCamera.cullingMask = layerMask;
    }
}
