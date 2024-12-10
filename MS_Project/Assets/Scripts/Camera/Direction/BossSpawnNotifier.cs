using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnNotifier : MonoBehaviour
{
    // シングルトン
    public static BossSpawnNotifier instance;
    public static BossSpawnNotifier Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("BossSpawnNotifier");
                instance = go.AddComponent<BossSpawnNotifier>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    // オブザーバー
    private List<IBossSpawnObserver> _observers = new List<IBossSpawnObserver>();

    /// <summary>
    /// オブザーバーを追加
    /// </summary>
    /// <param name="observer"></param>
    public void AddObserver(IBossSpawnObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    /// <summary>
    /// オブザーバーを削除
    /// </summary>
    public void RemoveObserver(IBossSpawnObserver observer)
    {
        _observers.Remove(observer);
    }


    /// <summary>
    /// ボスがスポーンしたことを通知
    /// </summary>
    public void NotifyBossSpawned(GameObject boss)
    {
        foreach (var observer in _observers)
        {
            observer.OnBossSpawned(boss);
        }
    }
}
