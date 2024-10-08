using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnomatoManager : MonoBehaviour
{
    //  モードチェンジのイベント
    public delegate void OnomatoEventHandler(PlayerMode mode);
    public static event OnomatoEventHandler OnModeChangeEvent;

    // 暴走ゲージを溜めるイベント
    public delegate void FrenzyEventHandler(float amount);
    public static event FrenzyEventHandler OnIncreaseFrenzyEvent;

    //private void OnEnable()
    //{
    //    //イベントをバインドする
    //    AttackColliderManager.OnOnomatoEvent += Absorb;
    //}

    //private void OnDisable()
    //{
    //    //バインドを解除する
    //    AttackColliderManager.OnOnomatoEvent -= Absorb;
    //}

    /// <summary>
    /// 食べられる処理
    /// </summary>
    public void Absorb()
    {
        Debug.Log("OnomatoManager:イベントを受信、モードチェンジ" + transform.position);
        //モードチェンジのイベント送信
        OnModeChangeEvent?.Invoke(PlayerMode.Hammer);

        //暴走ゲージを溜めるイベント送信
        OnIncreaseFrenzyEvent?.Invoke(5.0f);

    }
}
