using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HP処理インタフェース
/// </summary>
public interface ILife
{
    /// <summary>
    /// ダメージ処理
    /// </summary>
    void TakeDamage(float _damage);

    /// <summary>
    /// 死亡処理
    /// </summary>
    void OnDeath();

    float Health { get; set; }
}
