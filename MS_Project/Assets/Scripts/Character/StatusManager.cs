using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのステータスを管理するビヘイビア
/// </summary>
public class StatusManager : MonoBehaviour, ILife
{
    [SerializeField, Header("ステータスデータ")]
    protected CharacterStatusData statusData;

    [SerializeField, Tooltip("体力")]
    protected float currentHealth;

    protected virtual void Awake()
    {
        currentHealth = statusData.maxHealth;
    }

    public virtual void TakeDamage(float _damage)
    {
        currentHealth -= _damage;
       // Debug.Log("Damage:" + _damage);//test

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

    public virtual CharacterStatusData StatusData
    {
        get => statusData;
    }

    public virtual WorldObjectType ObjectType
    {
        get => statusData.ObjectType;
    }

}
