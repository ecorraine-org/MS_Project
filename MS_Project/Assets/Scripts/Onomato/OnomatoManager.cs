using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnomatoManager : MonoBehaviour
{
    // モードチェンジイベントのデリゲート定義
    public delegate void OnomatoEventHandler(PlayerMode mode);

    //  モードチェンジのイベント定義
    public static event OnomatoEventHandler OnModeChangeEvent;

    private void OnEnable()
    {
        //イベントをバインドする
        AttackColliderManager.OnOnomatoEvent += Absorb;
    }

    private void OnDisable()
    {
        //バインドを解除する
        AttackColliderManager.OnOnomatoEvent -= Absorb;
    }

    /// <summary>
    /// 食べられる処理
    /// </summary>
    public void Absorb()
    {
        Debug.Log("OnomatoManager:イベントを受信、モードチェンジ" + transform.position);
        //モードチェンジのイベント送信
        OnModeChangeEvent?.Invoke(PlayerMode.Hammer);
    }
}
