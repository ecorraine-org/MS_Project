using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    private Vector3 center = Vector3.up;
    [Tooltip("この範囲内に生成する")]
    private float radius;

    [Header("生成するエネミーリスト")]
    [Header("雑魚数"), Tooltip("生成する雑魚の最大数")]
    public int mobMaxCount = 0;
    [Header("雑魚種類"), Tooltip("生成する雑魚種類")]
    public List<EnemyStatusData> mobPool;
    private int mobCount = 0;

    [Header("エリート数"), Tooltip("生成するエリートの最大数")]
    public int eliteMaxCount = 0;
    [Header("エリート種類"), Tooltip("生成するエリート種類")]
    public List<EnemyStatusData> elitePool;
    private int eliteCount = 0;

    [Space]
    [Space]
    [Header("敵合計数（合計２５匹まで）")]
    public int totalEnemyCount = 0;

    public List<GameObject> enemyPool;
    private int maxPoolSize = 25;

    private void Awake()
    {
        radius = this.GetComponent<SphereCollider>().radius;

        totalEnemyCount = mobMaxCount + eliteMaxCount;
    }

    private void Start()
    {
        if (mobPool.Count > 0)
        {
            int random = Random.Range(0, mobPool.Count);
            Spawn(mobPool[random], RandomizeWithinRadius());
        }
        else
            CustomLogger.Log("No mobs to spawn.");
    }

    private void Update()
    {
    }

    public void Spawn(EnemyStatusData _enemydata, Vector3 _position)
    {
        /*
        enemyPool = new ObjectPool<GameObject>(
            OnCreatePoolObject,
            (b) => { b.gameObject.SetActive(true); },
            (b) => { b.gameObject.SetActive(false); },
            (b) => { Destroy(b.gameObject); },
            true,
            totalEnemyCount,
            maxPoolSize
        );
        */
        if (_enemydata == null)
            return;

        

    }

    GameObject IntantiateEnemy(EnemyStatusData _data, Vector3 _position)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Enemy/EnemyContainer"), _position, Quaternion.identity);
        obj.GetComponent<EnemyController>().status.StatusData = _data;
        return obj;
    }

    public Vector3 RandomizeWithinRadius()
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection *= Random.Range(0f, radius);
        randomDirection = randomDirection + center;
        randomDirection.y = center.y;

        return randomDirection;
    }
    /*
    void Initialize(Transform _container, MonoBehaviour _monoBehaviour);
    void Stop();
    void DespawnAll();
    */
}