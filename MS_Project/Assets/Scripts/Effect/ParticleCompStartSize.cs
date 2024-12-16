using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// パーティクルのライフタイム変更のイベント送信先ビヘイビア
/// </summary>
public class ParticleCompStartSize : ParticleCompBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    //固定値
    //public void SetStartSize(float size)
    //{
    //    var mainModule = particle.main;
    //    mainModule.startSize = new ParticleSystem.MinMaxCurve(size);
    //}

    //範囲
    public void SetStartSize(Vector2 size)
    {
        float minSize = size.x;
        float maxSize = size.y;
        var mainModule = particle.main;
        mainModule.startSize = new ParticleSystem.MinMaxCurve(minSize, maxSize);
    }

    //public void ChangeStartSize()
    //{
    //    //パラメータ取得
    //    var mainModule = particle.main;
    //    //新しいサイズを適用
    //  //  mainModule.startSize = chargeTime;

    //}


}
