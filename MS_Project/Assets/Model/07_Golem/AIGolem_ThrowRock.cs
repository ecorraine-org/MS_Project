using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGolem_ThrowRock : MonoBehaviour
{
    public GameObject rockPrefab; // 投げるプレハブ
    public Transform spawnPoint; // プレハブの生成位置
    public float throwForce = 10f; // 投げる力
    bool haveRock = false;

    GameObject thrownRock;
    Rigidbody rockRb; // 投げる岩のもの

    public void Update()
    {
        if(haveRock)
        {//spawnPointに座標を保持
            thrownRock.transform.position = spawnPoint.position;
        }
    }

    // アニメーションイベントから呼び出すメソッド
    public void ThrowRock()
    {
        haveRock = false;
        if (rockPrefab != null && spawnPoint != null)
        {
            

            if (rockRb != null)
            {
                // Playerタグのオブジェクトを探す
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    // プレイヤーの位置方向へのベクトルを計算
                    Vector3 direction = (player.transform.position - spawnPoint.position).normalized;

                    // 前に飛ばす準備
                    //Vector3 forwardDirection = enemy.transform.forward;
                    //forwardDirection.y = 0f;// 地面に水平に
                    //
                    //Vector3 forceDirection = forwardDirection.normalized;
                    //
                    //throwForce *= forceDirection;

                    // プレイヤーに向けて力を加える
                    rockRb.AddForce(direction * throwForce, ForceMode.Impulse);
                }
                else
                {
                    Debug.LogWarning("Playerタグのオブジェクトが見つかりませんでした。");
                }
            }
        }
    }

    //岩を召喚
    public void SummonRock()
    {
        // プレハブのインスタンスを生成
        thrownRock = Instantiate(rockPrefab, spawnPoint.position, spawnPoint.rotation);
        rockRb = thrownRock.GetComponent<Rigidbody>();
        haveRock = true;
    }

}
