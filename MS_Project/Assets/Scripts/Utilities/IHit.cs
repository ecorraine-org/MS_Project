using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 被撃処理インタフェース
/// </summary>
public interface IHit
{
    /// <summary>
    /// 被撃処理
    /// </summary>
    /// <param name="canOneHitKill">直接殺せるか</param>
    void Hit(bool _canOneHitKill);
}
