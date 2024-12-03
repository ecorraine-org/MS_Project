using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのステータスを管理するビヘイビア
/// </summary>
public class StatusManager : MonoBehaviour, ILife
{
    [SerializeField, Header("ステータスデータ")]
    protected BaseStatusData statusData;

    [SerializeField, Tooltip("無敵かどうか")]
    protected bool isInvincible;

    [SerializeField, Tooltip("体力")]
    protected float currentHealth;

    protected virtual void Awake()
    {
        isInvincible = statusData.isInvincible;
        currentHealth = statusData.maxHealth;
    }

    public virtual void TakeDamage(float _damage)
    {
        if (isInvincible) return;

        currentHealth -= _damage;
       //Debug.Log("Damage:" + _damage);    // test

        if (currentHealth <= 0)
        {
            // 死亡
        }
    }

    public void OnDeath() { }

    /// <summary>
    /// 無敵かどうか
    /// </summary>
    public bool IsInvincible
    {
        get => isInvincible;
        set { isInvincible = value; }
    }

    /// <summary>
    /// 現在のＨＰ
    /// </summary>
    public float CurrentHealth
    {
        get => currentHealth;
        set { currentHealth = value; }
    }

    public virtual BaseStatusData StatusData
    {
        get => statusData;
    }
}
