using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg_Explosion : MonoBehaviour
{
    public GameObject impactEffect; // エフェクトプレハブ

    // OnTriggerEnterはトリガーに侵入した時に呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        // 衝突したオブジェクトのタグを取得
        string tag = other.gameObject.tag;

        // タグが"Ground"または"Player"の場合
        if (tag == "Ground" || tag == "Player")
        {
            // エフェクトを生成
            Instantiate(impactEffect, transform.position, Quaternion.identity);
            Debug.Log("エフェクト再生");
            // 衝突したら弾を消す
            Destroy(gameObject);
        }


    }
}
