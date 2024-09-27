using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̃X�e�[�^�X���Ǘ�����r�w�C�r�A
/// </summary>
public class StatusManager : MonoBehaviour, ILife
{
    [SerializeField, Header("�X�e�[�^�X�f�[�^")]
    protected CharaStatusData statusData;

    [SerializeField, Header("�̗�")]
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
            //���S

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
