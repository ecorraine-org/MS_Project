using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct BuffEffectInfo
{
    public float damageUpRate;
    public float speedUpRate;
    public float healValue;
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

    //速度バフ時間経過表示用コルーチン
    private Coroutine testSpeedCoroutine;

    [SerializeField, NonEditable, Header("一回回復持続時間")]
    float healDuration = 0.3f;

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

    void ApplyDamageBuff(OnomatopoeiaData _data)
    {

        //damageBuff
        if (_data.damageBuff != 1 && _data.damageBuff != 0)
        {

            //バフ効果      
            buffEffect.damageUpRate = _data.damageBuff;

         

            //バフない状態でバフを付ける
            if (damageCoroutine == null)
            {
                //エフェクト生成
                playerController.EffectManager.GenerateDamageBuffEffect();
            }

            StartBuffTimer(ref damageCoroutine, _data.buffDuration, () => EndDamageBuff());
        }
    }

    void ApplySpeedBuff(OnomatopoeiaData _data)
    {
        //speedBuff
        if (_data.speedBuff != 1 && _data.speedBuff != 0)
        {

            buffEffect.speedUpRate = _data.speedBuff;


            //バフない状態でバフを付ける
            if (speedCoroutine == null)
            {

                //エフェクト生成
                playerController.EffectManager.GenerateSpeedBuffEffect();

                //リセット
                testSpeedBuffTimer = 0;

                testSpeedCoroutine = TimerUtility.FrameBasedTimer(this, _data.buffDuration * 99, () => { testSpeedBuffTimer += Time.deltaTime; });
            }

            StartBuffTimer(ref speedCoroutine, _data.buffDuration, () => EndSpeedBuff());



        }
    }

    void ApplyHealBuff(OnomatopoeiaData _data)
    {
        if ( _data.healBuff != 0)
        {

            buffEffect.healValue = _data.healBuff;

      
            //バフない状態でバフを付ける
            if (healCoroutine == null)
            {
                //エフェクト生成
                playerController.EffectManager.GenerateHealBuffEffect();

            }


            // あれば停止させて、0から計算する
            if (healCoroutine != null)
            {
                StopCoroutine(healCoroutine);
            }

            healCoroutine = TimerUtility.FrameBasedTimer(this, healDuration, ()=> UpdateHeal(),()=>EndHealBuff());
     
        }
    }

    /// <summary>
    /// 徐々に回復
    /// </summary>
    private void UpdateHeal()
    {
  　   //フレームごとの回復量
       float healValue= (buffEffect.healValue / healDuration) * Time.deltaTime;

        playerController.StatusManager.TakeDamage(-healValue);
    }

    private void ApplyBuff(OnomatopoeiaData _data)
    {
        ApplyDamageBuff(_data);

        ApplyHealBuff(_data);

        ApplySpeedBuff(_data);
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
        buffEffect.healValue = 0;
        healCoroutine = null;

        //バフエフェクトを消す
        playerController.EffectManager.DestroyHealBuffEffect();
    }

    void EndSpeedBuff()
    {
        if (testSpeedCoroutine != null)
        {
            StopCoroutine(testSpeedCoroutine);
            testSpeedCoroutine = null;
        }

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
