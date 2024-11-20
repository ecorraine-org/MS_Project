using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using Unity.VisualScripting;

public class EnemySpawner : MonoBehaviour
{
    [TextArea]
    public string missionName = "";

    private Vector3 center = Vector3.up;

    [SerializeField, Header("この範囲内に生成を始める"), Tooltip("この範囲内に生成を始める")]
    private GameObject triggerArea;
    private float triggerRadius;
    [SerializeField, Header("この範囲内に生成する"), Tooltip("この範囲内に生成する")]
    private GameObject spawnArea;
    private float spawnRadius;

    [Space(20), Header("-----生成するエネミー情報-----")]
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

    private bool hasSpawned;
    private bool hasCleared;
    private UIMissionController mission;

    private Collector enemyCollector;

    private void OnValidate()
    {
        center = this.transform.position + Vector3.up;
        triggerRadius = triggerArea.GetComponent<SphereCollider>().radius;
        spawnRadius = spawnArea.GetComponent<SphereCollider>().radius;
    }

    private void Awake()
    {
        center = this.transform.position + Vector3.up;
        triggerRadius = triggerArea.GetComponent<SphereCollider>().radius;
        spawnRadius = spawnArea.GetComponent<SphereCollider>().radius;

        enemyCollector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject.GetComponent<Collector>();

        mission = GameObject.FindGameObjectWithTag("Mission").gameObject.GetComponent<UIMissionController>();
    }

    private void Start()
    {
        hasSpawned = false;
        hasCleared = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (hasSpawned) return;

        if (mobPool.Count <= 0 && elitePool.Count <= 0)
        {
            CustomLogger.Log("配列が空のため、スポーンする敵がありません。");
            return;
        }

        if (mobPool.Count > 0)
        {
            for (mobCount = 0; mobCount < mobMaxCount; mobCount++)
            {
                int random = Random.Range(0, mobPool.Count);
                Spawn(mobPool[random], RandomizeWithinRadius());
            }
        }
        if (elitePool.Count > 0)
        {
            for (eliteCount = 0; eliteCount < eliteMaxCount; eliteCount++)
            {
                int random = Random.Range(0, elitePool.Count);
                Spawn(elitePool[random], RandomizeWithinRadius());
            }
        }

        hasSpawned = true;

        if (mission)
        {
            CustomLogger.Log(this.gameObject.scene.name + "のスポナー起動");

            mission.Spawner = this;
            mission.MissionItem.SetActive(true);
            mission.MissionText = missionName;
        }
    }

    private void Update()
    {
    }

    public void Spawn(EnemyStatusData _enemydata, Vector3 _position)
    {
        if (_enemydata == null || enemyCollector.totalEnemyCount >= enemyCollector.MaxPoolSize)
        {
            CustomLogger.Log("スポーンが失敗しました。");
            return;
        }

        enemyCollector.enemyPool.Add(InstantiateEnemy(_enemydata, _position));
        enemyCollector.totalEnemyCount++;
    }

    GameObject InstantiateEnemy(EnemyStatusData _data, Vector3 _position)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Enemy/EnemyContainer"), _position, Quaternion.identity, enemyCollector.transform);
        obj.GetComponent<EnemyController>().EnemyStatus.StatusData = _data;
        return obj;
    }

    public Vector3 RandomizeWithinRadius()
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection *= Random.Range(0f, spawnRadius);
        randomDirection = randomDirection + center;
        randomDirection.y = center.y;

        return randomDirection;
    }

    /*
    void Initialize(Transform _container, MonoBehaviour _monoBehaviour);
    void Stop();
    void DespawnAll();
    */

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, triggerRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, spawnRadius);
    }
    #endregion
}