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
    public static Coroutine FrameBasedTimer(MonoBehaviour monoBehaviour, float time, Action onTick, Action onComplete)
    {
        return monoBehaviour.StartCoroutine( FrameBasedTimerCoroutine(time, onTick, onComplete));
    }

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
    #endregion
}
