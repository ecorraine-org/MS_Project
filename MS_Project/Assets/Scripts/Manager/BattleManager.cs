using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトル関連
/// </summary>
public class BattleManager : SingletonBaseBehavior<BattleManager>
{
    [SerializeField, Header("ヒットリアクションデータ")]
    PlayerHitData playerHitData;

    [Tooltip("プレイヤーモード")]
    PlayerMode curPlayerMode;

    bool isHitStop = false;

    public float slowSpeed;         //ヒットストップによる減速
    public float stopDuration;      //ヒットストップ持続時間

    protected override void AwakeProcess()
    {
        if (playerHitData == null)
        {
            Debug.LogError("playerHitDataが設定されていません", this);
        }
    }

    /// <summary>
    /// ヒットストップのコルーチンを開始
    /// </summary>
    public void StartHitStop(Animator _animator)
    {
        if (GetPlayerHitReaction().stopDuration == 0.0f) return;
        //StartCoroutine(HitStopCoroutine(_animator, GetPlayerHitReaction().slowSpeed, GetPlayerHitReaction().stopDuration));
        StartCoroutine(HitStopCoroutine(_animator, slowSpeed, stopDuration));
    }

    public void StartHitStop(Animator _animator, float _slowSpeed, float _duration)
    {
        if (_duration == 0.0f) return;
        StartCoroutine(HitStopCoroutine(_animator, _slowSpeed, _duration));
    }

    private IEnumerator HitStopCoroutine(Animator _animator, float _slowSpeed, float _duration)
    {
        //流す速度を遅くする
        _animator.speed = _slowSpeed;
        isHitStop = true;

        yield return new WaitForSeconds(_duration);

        //流す速度を戻す
        if (_animator != null) _animator.speed = 1f;
        isHitStop = false;
    }

    #region TimeSlow

    /*
    public void StartTimeSlow(float _slowSpeed, float _duration)
    {
        StartCoroutine(TimeSlowCoroutine(_slowSpeed, _duration));
    }

    private IEnumerator TimeSlowCoroutine(float _slowSpeed, float _duration)
    {
        // 時間を遅くする
        Time.timeScale = _slowSpeed;

        yield return new WaitForSeconds(_duration);

        //  時間の流れを元に戻す
        Time.timeScale = 1;
    }
    */

    public void StartTimeSlow(float _slowSpeed, float _duration)
    {
        TimerUtility.TimeBasedTimer(this, _duration,
            () => SetTimeSlow(_slowSpeed),              //一回だけ実行
            () => ResetTime());                         //終了処理
    }

    private void SetTimeSlow(float _slowSpeed)
    {
        Time.timeScale = _slowSpeed;
    }

    private void ResetTime()
    {
        Time.timeScale = 1;
    }
    #endregion

    public HitReaction GetPlayerHitReaction()
    {
        return playerHitData.dicHitReac[curPlayerMode];
    }

    public PlayerHitData PlayerHitData
    {
        get => this.playerHitData;
    }

    public PlayerMode CurPlayerMode
    {
        get => this.curPlayerMode;
        set { this.curPlayerMode = value; }
    }

    public bool IsHitStop
    {
        get => this.isHitStop;
    }
}
