using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのステータスを管理するビヘイビア
/// </summary>
public class StatusManager : MonoBehaviour, ILife
{
    [SerializeField, Header("ステータスデータ")]
    protected CharaStatusData statusData;

    [SerializeField, Header("体力")]
    float currentHealth;

    protected virtual void Awake()
    {
        currentHealth = statusData.MaxHealth;
    }

    public virtual void TakeDamage(float _damage)
    {
        currentHealth -= _damage;
        Debug.Log("Damage:" + _damage);//test

        if (currentHealth <= 0)
        {
            //死亡

        }
    }

    public void OnDeath()
    {

    }

    public float Health
    {
        get => currentHealth;
        set { currentHealth = value; }
    }

    public CharaStatusData StatusData
    {
        get => statusData;

    }
}
