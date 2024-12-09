using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using Unity.VisualScripting;
using System.Diagnostics;

public class EnemySpawner : MonoBehaviour
{
    private GameObject enemyCollector;

    #region スポナー情報
    [SerializeField, Header("この範囲内に生成を始める"), Tooltip("この範囲内に生成を始める")]
    private GameObject triggerArea;
    private float triggerRadius;
    [SerializeField, Header("この範囲内に生成する"), Tooltip("この範囲内に生成する")]
    private GameObject spawnArea;
    private float spawnRadius;

    private Vector3 center = Vector3.up;
    #endregion

    #region ミッション情報
    [SerializeField, Header("ミッションタイプ"), Tooltip("ミッションタイプ")]
    private MissionType missionType = MissionType.None;
    [Tooltip("ミッション詳細")]
    private string missionDetail = "";
    [Tooltip("キル数")]
    private int killCount = 0;

    [SerializeField, Header("ミッション済み"), Tooltip("ミッション済み")]
    private bool hasCleared = false;
    private bool startMission = false;

    private UIMissionController mission;
    #endregion

    #region 生成情報
    [Space(20), Header("-----生成するエネミー情報-----")]
    [Header("雑魚数"), Tooltip("生成する雑魚の最大数")]
    public int mobMaxCount = 0;
    [Header("雑魚種類"), Tooltip("生成する雑魚種類")]
    public List<EnemyStatusData> mobList;
    private int mobCount = 0;

    [Header("エリート数"), Tooltip("生成するエリートの最大数")]
    public int eliteMaxCount = 0;
    [Header("エリート種類"), Tooltip("生成するエリート種類")]
    public List<EnemyStatusData> eliteList;
    private int eliteCount = 0;

    private bool hasFinishedSpawn = false;
    #endregion

    #region 生成されたエネミ
    [Space(20), Header("-----生成されたエネミー-----")]
    [Header("エネミープール（合計２５匹まで）"), Tooltip("エネミープール")]
    public List<GameObject> enemyPool;
    [Tooltip("敵合計数")]
    private int totalEnemyCount = 0;
    [Tooltip("最大プールサイズ")]
    private int maxPoolSize = 25;
    #endregion

    #region 生成されたオノマトペ
    [Header("生成されたオノマトペ"), Tooltip("生成されたオノマトペプール")]
    public List<GameObject> enemyOnomatoPool;
    [Header("最大オノマトペ数"), Tooltip("最大オノマトペ数")]
    public int maxEnemyOnomatopoeiaCount = 5;
    #endregion

    [Conditional("UNITY_EDITOR")]
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

        enemyCollector = this.transform.GetChild(1).gameObject;

        mission = GameObject.FindGameObjectWithTag("Mission").gameObject.GetComponent<UIMissionController>();
    }

    private void Start()
    {
        hasFinishedSpawn = false;
        startMission = false;
        hasCleared = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (hasFinishedSpawn) return;

        if (mobList.Count <= 0 && eliteList.Count <= 0)
        {
            CustomLogger.Log("配列が空のため、スポーンする敵がありません。");
            return;
        }

        if (mobList.Count > 0)
        {
            for (mobCount = 0; mobCount < mobMaxCount; mobCount++)
            {
                int random = Random.Range(0, mobList.Count);
                SpawnEnemy(mobList[random], RandomizeWithinRadius());
            }
        }
        if (eliteList.Count > 0)
        {
            for (eliteCount = 0; eliteCount < eliteMaxCount; eliteCount++)
            {
                int random = Random.Range(0, eliteList.Count);
                SpawnEnemy(eliteList[random], RandomizeWithinRadius());
            }
        }

        hasFinishedSpawn = true;

        if (mission)
        {
            CustomLogger.Log(this.gameObject.scene.name + "のスポナー起動");

            mission.Spawner = this;
            mission.MissionTitle.SetActive(true);
            mission.MissionItem.SetActive(true);

            startMission = true;
            /*
            string count = "<color=#00ff00>" + killCount + "/" + mobCount.ToString() + "</color>";
            missionDetail = count;
            missionDetail = mission.GetMissionDetails(missionType, missionDetail);
            */
        }
    }

    private void Update()
    {
        if(startMission && mission.MissionItem.activeInHierarchy)
        {
            string count = "<color=#00ff00>" + killCount + "/" + mobCount.ToString() + "</color>";
            missionDetail = count;
            missionDetail = mission.GetMissionDetails(missionType, missionDetail);

            if (killCount >= mobCount)
            {
                hasCleared = true;
                mission.MissionItem.SetActive(false);
                mission.MissionTitle.SetActive(false);
            }
        }
    }

    public void SpawnEnemy(EnemyStatusData _enemydata, Vector3 _position)
    {
        if (_enemydata == null || totalEnemyCount >= maxPoolSize)
        {
            CustomLogger.Log("スポーンが失敗しました。");
            return;
        }

        enemyPool.Add(InstantiateEnemy(_enemydata, _position));
        totalEnemyCount++;
    }

    private GameObject InstantiateEnemy(EnemyStatusData _data, Vector3 _position)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Enemy/EnemyContainer"), _position, Quaternion.identity, enemyCollector.transform);
        obj.GetComponent<EnemyController>().Status.StatusData = _data;
        obj.name = _data.name;
        obj.GetComponent<EnemyController>().ParentSpawner = this.gameObject;
        return obj;
    }

    private Vector3 RandomizeWithinRadius()
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection *= Random.Range(0f, spawnRadius);
        randomDirection = randomDirection + center;
        randomDirection.y = center.y;

        return randomDirection;
    }

    /// <summary>
    /// エネミーオブジェクトをプールから削除
    /// </summary>
    public void DespawnEnemyFromPool(GameObject _self)
    {
        killCount++;
        totalEnemyCount--;
        enemyPool.Remove(_self);
        Destroy(_self);
    }

    /// <summary>
    /// エネミーオブジェクトを全て削除
    /// </summary>
    public void DespawnAll()
    {
        foreach (var enemy in enemyPool)
        {
            DespawnEnemyFromPool(enemy);
        }
    }

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