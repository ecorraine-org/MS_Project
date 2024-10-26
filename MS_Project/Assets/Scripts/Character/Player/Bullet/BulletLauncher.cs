using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    [SerializeField, Header("発射する剣の軌跡プレハブ")]
    SpriteBullet spritePrefab;

    //スプライトの弾
    SpriteBullet spriteBullet;

    [SerializeField, Header(" 弾の初期位置")]
    Vector3 spawnPos;

    //親オブジェクトのスプライトレンダラー
    //  SpriteRenderer ownerSpriteRenderer;


    /// <summary>
    ///スプライトの弾を発射する
    /// </summary>
    public void SpriteFire(SpriteRenderer _spriteRenderer)
    {

        Vector3 adjustSpawnPos;

        //キャラクターによる発射位置調整
        if (_spriteRenderer.flipX)
        {
            adjustSpawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z);
        }
        else 
        {
            adjustSpawnPos = new Vector3(-1*spawnPos.x, spawnPos.y, spawnPos.z);
        }

        spriteBullet = Instantiate(this.spritePrefab, transform.position+ adjustSpawnPos, transform.rotation);
        spriteBullet.Init(_spriteRenderer.flipX);
    }

    private void Update()
    {

    }
}
