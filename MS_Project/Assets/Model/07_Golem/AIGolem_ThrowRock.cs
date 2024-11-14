using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGolem_ThrowRock : MonoBehaviour
{
    public GameObject rockPrefab; // 投げるプレハブ
    public Transform spawnPoint; // プレハブの生成位置
    public float throwForce = 10f; // 投げる力

    // アニメーションイベントから呼び出すメソッド
    public void ThrowRock()
    {
        if (rockPrefab != null && spawnPoint != null)
        {
            // プレハブのインスタンスを生成
            GameObject thrownRock = Instantiate(rockPrefab, spawnPoint.position, spawnPoint.rotation);
            // 投げる方向に力を加える
            Rigidbody rb = thrownRock.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(spawnPoint.forward * throwForce, ForceMode.Impulse);
            }
        }
    }
}
