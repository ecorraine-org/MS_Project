using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BuffEffectInfo
{
    public float damageUpRate;
    public float speedUpRate;
}

public class PlayerBuffManager : MonoBehaviour
{
    [SerializeField,NonEditable,Header("バフ情報")]
    BuffEffectInfo buffEffect;

    private void OnEnable()
    {
        //イベントをバインドする
        OnomatoManager.OnEatOnomatoEvt += ApplyBuff;
    }

    private void OnDisable()
    {
        //バインドを解除する
        OnomatoManager.OnEatOnomatoEvt -= ApplyBuff;
    }

    //PlayerControllerの参照
    PlayerController playerController;

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;

        InitBuff();
    }

    /// <summary>
    /// バフ初期化
    /// </summary>
    private void InitBuff()
    {
        buffEffect.damageUpRate = 1.0f;
        buffEffect.speedUpRate = 1.0f;
    }

    void ApplyBuff(OnomatopoeiaData _data)
    {
        //バフ効果
        Debug.Log("バフを付ける");
        //damageBuff
        buffEffect.damageUpRate = _data.damageBuff;

        //speedBuff
        buffEffect.speedUpRate = _data.speedBuff;

        //_data.healBuff

        //バフエフェクト生成

 

        TimerUtility.FrameBasedTimer (this, _data.buffDuration, null, () => EndBuff());  
    }

    void EndBuff()
    {
        Debug.Log("終了");

        //バフエフェクトを消す

        //バフ効果を無くす
        InitBuff();
    }

    public BuffEffectInfo BuffEffect
    {
        get =>buffEffect;
       // set {buffEffect=value; }
    }


}
