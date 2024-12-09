using UnityEngine;

/// <summary>
/// プレイヤーの移動範囲を制限するエリア
/// </summary>
public class PlayerBoundary : MonoBehaviour
{
    //最後の有効位置
    private Vector3 lastValidPosition;

    Transform player;

    bool isInArea;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            // 初期位置記録
            lastValidPosition = player.position; 
        }
    }

    void FixedUpdate()
    {

        if (player != null && IsInsideBoundary(player.position))
        {
            //位置記録
            lastValidPosition = player.position;
            isInArea=true;
        }

        //元の位置に戻す
        if(!IsInsideBoundary(player.position)&& isInArea) 
            player.position = lastValidPosition;

    }

    /// <summary>
    /// 位置が境界内にあるかを判定する
    /// </summary>
    private bool IsInsideBoundary(Vector3 position)
    {
        // Collider.ClosestPoint を使用して、プレイヤーの位置に最も近い点を取得する
        Vector3 closestPoint = GetComponent<Collider>().ClosestPoint(position);

        // プレイヤーの位置と最も近い点との距離が、胶囊体の半径より大きければ、プレイヤーは境界を越えていると判断する
        float distanceToClosestPoint = Vector3.Distance(position, closestPoint);

        // 距離がある閾値（胶囊体の半径）を超えている場合、プレイヤーは境界外に出ていると見なす
        return distanceToClosestPoint < 0.1f; // この閾値は胶囊のサイズに応じて調整可能
    }


    //void OnTriggerExit(Collider other)
    //{
    //    if (player != null && other.CompareTag("Player"))
    //    {
    //        Debug.Log("制限");
    //        //元の位置に戻す
    //        other.transform.position = lastValidPosition;
    //    }


    //}
}
