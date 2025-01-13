using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガーベージコレクター
/// </summary>
public class ObjectCollector : MonoBehaviour
{
    private static ObjectCollector instance;
    public static ObjectCollector Instance => instance;

    private GameObject owner;

    [Header("オブジェクトプール"), Tooltip("オブジェクトプール")]
    public List<GameObject> otherObjectPool;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (otherObjectPool.Count > 50)
        {
            Destroy(otherObjectPool[0]);
            otherObjectPool.RemoveAt(0);
        }
    }

    /*
    public void DespawnEnemyFromPool(GameObject _self)
    {
        enemyPool.Remove(_self);
        Destroy(_self);
    }
    */

    public void DestroyOtherObjectFromPool(GameObject _self)
    {
        otherObjectPool.Remove(_self);
        Destroy(_self);
    }

    public GameObject Owner
    {
        get => owner;
        set { owner = value; }
    }
}
