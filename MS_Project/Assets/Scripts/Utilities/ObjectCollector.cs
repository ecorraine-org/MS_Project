using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガーベージコレクター
/// </summary>
public class ObjectCollector : MonoBehaviour
{
    /*
    [Header("敵合計数（合計２５匹まで）")]
    public int totalEnemyCount = 0;

    [Header("エネミープール"), Tooltip("エネミープール")]
    public List<GameObject> enemyPool;
    private int maxPoolSize = 25;
    */
    private GameObject owner;

    [Header("オブジェクトプール"), Tooltip("オブジェクトプール")]
    public List<GameObject> otherObjectPool;

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
