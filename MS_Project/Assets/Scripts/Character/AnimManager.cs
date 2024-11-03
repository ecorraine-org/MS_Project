using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションイベントを管理するビヘイビア
/// </summary>
public abstract class AnimManager : MonoBehaviour
{
    //アニメーション終了したか
    protected bool isAnimEnd = false;

    public bool IsAnimEnd
    {
        get => this.isAnimEnd;
    }

    /// <summary>
    /// ステート遷移時のリセット処理
    /// </summary>
    public void Reset()
    {
        isAnimEnd = false;
    }

    /// <summary>
    /// アニメーション終了フラグ設定
    /// </summary>
    public void EndAnim()
    {
        isAnimEnd = true;
    }

    /// <summary>
    /// 連撃フレーム
    /// </summary>
    public abstract void EnableCombo();

    /// <summary>
    /// 攻撃可能設定
    /// </summary>
    public abstract void EnableHit();


    /// <summary>
    /// 攻撃不可設定
    /// </summary>
    public abstract void DisableHit();

    /// <summary>
    /// 突進開始
    /// </summary>
    public abstract void StartDash();

    /// <summary>
    /// 突進終了
    /// </summary>
    public abstract void EndDash();
}
