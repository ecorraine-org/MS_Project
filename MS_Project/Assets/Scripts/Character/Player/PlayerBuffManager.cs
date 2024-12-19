using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct BuffEffectInfo
{
    public float damageUpRate;
    public float speedUpRate;
    //public bool hasDamageBuff;//バフ中かどうか
    //public bool hasSpeedBuff;
}

public class PlayerBuffManager : MonoBehaviour
{
    [SerializeField, NonEditable, Header("バフ情報")]
    BuffEffectInfo buffEffect;

    //攻撃力バフコルーチン
    private Coroutine damageCoroutine;

    //速度バフコルーチン
    private Coroutine speedCoroutine;

    //回復バフコルーチン
    private Coroutine healCoroutine;

    private Coroutine testCoroutine;

   
    //消えるまで(食べた後を継続を含めて)の持続時間
    public float testSpeedBuffTimer;

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

    private void StartBuffTimer(ref Coroutine _coroutine, float _duration, Action onComplete)
    {
        // あれば停止させる
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
       

        _coroutine = TimerUtility.FrameBasedTimer(this, _duration, null, onComplete);


        

    }

    void ApplyBuff(OnomatopoeiaData _data)
    {

        //_data.healBuff

        //damageBuff
        if (_data.damageBuff != 1 && _data.damageBuff != 0)
        {

            //バフ効果      
            buffEffect.damageUpRate = _data.damageBuff;

            StartBuffTimer(ref damageCoroutine, _data.buffDuration, () => EndDamageBuff());

            //バフない状態でバフを付ける
            if (damageCoroutine == null)
            {
                //エフェクト生成
                //playerController.EffectManager.GenerateDamageBuffEffect();
            }
        }

        //speedBuff
        if (_data.speedBuff != 1 && _data.speedBuff != 0)
        {
          //  effectManager.GenerateBuffEffect(PlayerEffect.SpeedBuff);

            buffEffect.speedUpRate = _data.speedBuff;
            Debug.Log("バフを付ける " + _data.speedBuff + " 現在　" + buffEffect.speedUpRate);


            //バフない状態でバフを付ける
            if (speedCoroutine == null)
            {

                //エフェクト生成
                playerController.EffectManager.GenerateSpeedBuffEffect();

                //リセット
                testSpeedBuffTimer = 0;

                testCoroutine = TimerUtility.FrameBasedTimer(this, _data.buffDuration * 99, () => { testSpeedBuffTimer += Time.deltaTime; });
            }

            StartBuffTimer(ref speedCoroutine, _data.buffDuration, () => EndSpeedBuff());



           
        }


       


    }

    void EndDamageBuff()
    {
        //バフ効果を無くす
        buffEffect.damageUpRate = 1;
        damageCoroutine = null;

        //バフエフェクトを消す
        playerController.EffectManager.DestroyDamageBuffEffect();
    }

    void EndHealBuff()
    {
        //バフ効果を無くす
       // buffEffect.damageUpRate = 1;
        healCoroutine = null;

        //バフエフェクトを消す
        playerController.EffectManager.DestroyHealBuffEffect();
    }

    void EndSpeedBuff()
    {    
        if (testCoroutine != null)
        {
            StopCoroutine(testCoroutine);
            testCoroutine = null;
        }
    
       // Debug.Log("END");
        //バフ効果を無くす
        buffEffect.speedUpRate = 1;
        speedCoroutine = null;

        //バフエフェクトを消す
        playerController.EffectManager.DestroySpeedBuffEffect();
    }

    public BuffEffectInfo BuffEffect
    {
        get => buffEffect;
        // set {buffEffect=value; }
    }


}
