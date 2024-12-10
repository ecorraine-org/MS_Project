using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シングルトンベースビヘイビア
/// </summary>
public abstract class SingletonBaseBehavior<T> : MonoBehaviour
    where T : Component
{
    public static T Instance { get; private set; }

    public virtual void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this as T;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        //新しいシーンに行っても消えないように
        //DontDestroyOnLoad(gameObject); 

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
        //新しいシーンに行っても消えないように
        DontDestroyOnLoad(gameObject); 

        this.AwakeProcess();
    }

    protected abstract void AwakeProcess();
}
