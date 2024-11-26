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
            Rigidbody rb = thrownRock.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Playerタグのオブジェクトを探す
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    // プレイヤーの位置方向へのベクトルを計算
                    Vector3 direction = (player.transform.position - spawnPoint.position).normalized;

                    // プレイヤーに向けて力を加える
                    rb.AddForce(direction * throwForce, ForceMode.Impulse);
                }
                else
                {
                    Debug.LogWarning("Playerタグのオブジェクトが見つかりませんでした。");
                }
            }
        }
    }
}
