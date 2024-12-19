using UnityEngine;
using System;
using System.Collections;
/// <summary>
/// タイマーのユーティリティクラス
/// </summary>
public static class TimerUtility
{
    /// <summary>
    /// コルーチンを呼び出す(事前処理なし)
    /// </summary>
    /// <param name="monoBehaviour">基本this</param>
    /// <param name="time">タイマーの継続時間（秒）</param>
    /// <param name="onComplete">指定時間経過後に呼び出される処理 eg:() => Func()</param>
    /// <returns></returns>
    public static Coroutine TimeBasedTimer(MonoBehaviour monoBehaviour, float time, Action onComplete)
    {
        return monoBehaviour.StartCoroutine(TimeBasedTimerCoroutine(time, onComplete));
    }

    /// <summary>
    /// コルーチンを呼び出す(事前処理一回だけ実行する)
    /// </summary>
    public static Coroutine TimeBasedTimer(MonoBehaviour monoBehaviour, float time, Action onStart, Action onComplete)
    {
        return monoBehaviour.StartCoroutine(TimeBasedTimerCoroutine(time, onStart, onComplete));
    }


    /// <summary>
    /// コルーチンを呼び出す( フレーム毎に事前処理を呼び出す)
    /// </summary>
    /// <param name="monoBehaviour">基本this</param>
    /// <param name="time">タイマーの継続時間（秒）</param>
    /// <param name="onTick">毎フレーム呼び出される処理 eg:() => Func()</param>
    /// <param name="onComplete">指定時間経過後に呼び出される処理</param>
    /// <returns></returns>
    public static Coroutine FrameBasedTimer(MonoBehaviour monoBehaviour, float time, Action onTick = null, Action onComplete = null)
    {
        return monoBehaviour.StartCoroutine( FrameBasedTimerCoroutine(time, onTick, onComplete));
    }

    public static Coroutine FrameBasedTimerFixed(MonoBehaviour monoBehaviour, float time, Action onTick, Action onComplete)
    {
        return monoBehaviour.StartCoroutine(FrameBasedTimerCoroutineFixed(time, onTick, onComplete));
    }

    /// <summary>
    /// 動的に時間を変更できるタイマーのコルーチンを呼び出す
    /// </summary>
    /// <param name="monoBehaviour">基本this</param>
    /// <param name="getTime">現在のターゲット時間を返す関数 eg:() => targetTime targetTimeはfloat型変数</param>
    /// <param name="onTick">毎フレーム呼び出される処理</param>
    /// <param name="onComplete">指定時間経過後に呼び出される処理</param>
    /// <returns></returns>
    //public static Coroutine DynamicFrameBasedTimer(MonoBehaviour monoBehaviour, Func<float> getTime, Action onTick = null, Action onComplete = null)
    //{
    //    return monoBehaviour.StartCoroutine(DynamicFrameBasedTimerCoroutine(getTime, onTick, onComplete));
    //}

    // コルーチン実装部分
    #region Coroutine Methods

    /// <summary>
    /// 時間経過をWaitForSecondsで処理するコルーチン（事前処理一回だけ実行する）
    /// </summary>
    private static IEnumerator TimeBasedTimerCoroutine(float time, Action beforeComplete, Action onComplete)
    {
        beforeComplete();

        yield return new WaitForSeconds(time);

        // 時間経過後に呼ばれるコールバック
        onComplete?.Invoke();
    }

    /// <summary>
    /// 時間経過をWaitForSecondsで処理するコルーチン(事前処理なし)
    /// </summary>
    private static IEnumerator TimeBasedTimerCoroutine(float time, Action onComplete)
    {
        yield return new WaitForSeconds(time);

        // 時間経過後に呼ばれるコールバック
        onComplete?.Invoke();
    }

    /// <summary>
    /// フレーム毎に時間を経過させるコルーチン
    /// </summary>
    private static IEnumerator FrameBasedTimerCoroutine(float time, Action beforeComplete, Action onComplete)
    {
        //一回だけ実行する
        float elapsedTime = 0f;

        // 時間が経過するまでフレーム毎に繰り返す
        while (elapsedTime < time)
        {
            // 進行中に毎フレーム呼ばれるコールバック
            beforeComplete?.Invoke();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 時間経過後に呼ばれるコールバック
        onComplete?.Invoke();
    }

    private static IEnumerator FrameBasedTimerCoroutineFixed(float time, Action beforeComplete, Action onComplete)
    {
        float elapsedTime = 0f;

        // 時間が経過するまで繰り返す
        while (elapsedTime < time)
        {
            //進行中に毎FixedUpdateで呼ばれるコールバック
            beforeComplete?.Invoke();

            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // 時間経過後に呼ばれるコールバック
        onComplete?.Invoke();
    }




 

    /// <summary>
    /// 動的タイマーのコルーチン部分
    /// </summary>
    private static IEnumerator DynamicFrameBasedTimerCoroutine(Func<float> getTime, Action onTick, Action onComplete)
    {
        float elapsedTime = 0f;

        while (elapsedTime < getTime())
        {
            // 毎フレーム呼び出される処理
            onTick?.Invoke();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 時間経過後に呼び出される処理
        onComplete?.Invoke();
    }
    #endregion
}
