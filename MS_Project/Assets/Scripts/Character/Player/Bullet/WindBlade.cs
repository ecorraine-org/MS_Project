using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlade : SpriteBullet
{
    ParticleManager particleManager;

    [SerializeField, Header("エフェクトスケール")]
    float windBladeScale =0.5f;

    public override void Init(bool _isFlipX)
    {
        base.Init(_isFlipX);
        
        spriteRenderer.flipX = !_isFlipX;
        //親オブジェクトの向きによる反転処理
        Vector3 currentEulerAngles = transform.rotation.eulerAngles;
        if (_isFlipX)
        {
            transform.rotation = Quaternion.Euler(currentEulerAngles.x, -currentEulerAngles.y, currentEulerAngles.z);
        }

        float randomZRotation = Random.Range(-70f, 70f);
        //ランダム回転
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, randomZRotation);


        //パーティクル処理
        particleManager = GetComponentInChildren<ParticleManager>();
        particleManager.ChangeScale(windBladeScale);
}


}
