using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private Transform player;

    // 初期化（PlayerのTransformを渡す）
    public void Init()
    {
        // PlayerオブジェクトをHierarchyから取得
        player = GameObject.Find("Player")?.transform; // "Player"は実際のPlayerオブジェクトの名前に変更してください
    }

    // 衝突時に呼ばれるメソッド
    private void OnCollisionEnter(Collision collision)
    {
        // Playerに衝突した場合、オブジェクトを削除
        if (collision.transform == player)
        {
            Destroy(gameObject); // 自分自身（卵オブジェクト）を削除
        }
    }
}
