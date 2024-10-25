using UnityEngine;

public class AnimThrowEgg : MonoBehaviour
{
    public GameObject eggPrefab; // 投げるプレハブ
    public Transform spawnPoint; // プレハブの生成位置
    public float throwForce = 10f; // 投げる力

    // アニメーションイベントから呼び出すメソッド
    public void ThrowEgg()
    {
        if (axePrefab != null && spawnPoint != null)
        {
            // プレハブのインスタンスを生成
            GameObject thrownEgg = Instantiate(axePrefab, spawnPoint.position, spawnPoint.rotation);
            // 投げる方向に力を加える
            Rigidbody rb = thrownEgg.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(spawnPoint.forward * throwForce, ForceMode.Impulse);
            }
        }
    }
}
