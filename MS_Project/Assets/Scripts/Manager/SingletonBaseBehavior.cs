using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �V���O���g���x�[�X�r�w�C�r�A
/// </summary>
public abstract class SingletonBaseBehavior<T> : MonoBehaviour
    where T : Component
{
    public static T Instance { get; private set; }

    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }

        this.AwakeProcess();
    }

    protected abstract void AwakeProcess();
}
