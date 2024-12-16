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
    [SerializeField, Header("リザルトプレハブ")]
    GameObject resultPrefab;
    private bool isTransitioning = false;
    ResultScreenShot _resultScreenShot;

    [SerializeField, Header("ミッションエリアプレハブ")]
    PlayerBoundary missionArea;

    //生成されたミッションエリアを格納する
    GameObject missionAreaInstance;

    //ミッションエリア用のビヘイビア
    PlayerBoundary missionAreaComp;

    [SerializeField, Header("ミッションエリア範囲")]
    Vector3 missionAreaScale = new Vector3(1, 1, 1);

    //ミッションエリアサイズの初期値を記録
    //  Vector3 areaDefaultScale;

    [SerializeField, Header("ミッションタイプ"), Tooltip("ミッションタイプ")]
    private MissionType missionType = MissionType.None;
    [Tooltip("ミッション詳細")]
    private string missionDetail = "";
    [SerializeField, NonEditable, Header("キル数")]
    private int killCount = 0;

    [SerializeField, NonEditable, Header("ミッション済み")]
    private bool hasCleared = false;
    [SerializeField, NonEditable, Header("ミッション開始")]
    private bool isStartMission = false;

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
        isStartMission = false;
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
            CustomLogger.Log(this.gameObject.scene.name + "のスポナー" + name + "起動");

            mission.Spawner = this;
            mission.MissionTitle.SetActive(true);
            //if (!mission.MissionPanel.activeSelf)
            //{
            //    //子オブジェクト含めて、有効にする
            //    mission.MissionPanel.SetActive(true);
            //    // 一旦子オブジェクトを全部無効にして、条件によって有効にする
            //    mission.EnemyMissionItem.SetActive(false);
            //    mission.BossMissionItem.SetActive(false);
            //}


            if (missionType == MissionType.KillAll) mission.EnemyMissionItem.SetActive(true);
            if (missionType == MissionType.KillBoss) mission.BossMissionItem.SetActive(true);


            isStartMission = true;

            //ミッションエリア生成
            missionAreaInstance = Instantiate(missionArea.gameObject, transform.position, Quaternion.identity);
            missionAreaComp = missionAreaInstance.GetComponent<PlayerBoundary>();
            //元のサイズを記録
            // missionArea.DefaultScale = missionAreaInstance.transform.localScale;

            // string count = "<color=#00ff00>" + killCount + "/" + mobCount.ToString() + "</color>";
            //string count =  killCount + "/" + mobCount.ToString();
            //UnityEngine.Debug.Log("count "+ count);
            //missionDetail = count;
            //missionDetail = mission.GetMissionDetails(missionType, missionDetail);

            ////表示する
            //if (missionType == MissionType.KillAll) mission.EnemyTxt.text = count;
            //if (missionType == MissionType.KillBoss) mission.BossTxt.text = count;

            UpdateText();
        }
    }

    private void UpdateText()
    {
        // string count = "<color=#00ff00>" + killCount + "/" + mobCount.ToString() + "</color>";
        string count = killCount + "/" + mobCount.ToString();
        UnityEngine.Debug.Log("count " + count);
        missionDetail = count;
        missionDetail = mission.GetMissionDetails(missionType, missionDetail);

        //表示する
        if (missionType == MissionType.KillAll) mission.EnemyTxt.text = count;
        if (missionType == MissionType.KillBoss) mission.BossTxt.text = count;
    }

    private void Update()
    {

        if (missionAreaInstance != null && missionAreaComp != null)
        {
            //エリアサイズ変更
            missionAreaComp.ChangeScale(missionAreaScale);
        }

        // if (isStartMission&& !hasCleared && mission.MissionPanel.activeInHierarchy)
        if (isStartMission && !hasCleared)
        {
            string count = "<color=#00ff00>" + killCount + "/" + mobCount.ToString() + "</color>";
            missionDetail = count;
            missionDetail = mission.GetMissionDetails(missionType, missionDetail);

            if (killCount >= mobCount)
            {
                hasCleared = true;
                isStartMission = false;
                // mission.MissionPanel.SetActive(false);
                mission.MissionTitle.SetActive(false);

                if (missionType == MissionType.KillAll)
                {
                    mission.EnemyMissionItem.SetActive(false);
                }

                //ボスを倒したら、リザルト画面を出す
                if (missionType == MissionType.KillBoss)
                {
                    mission.BossMissionItem.SetActive(false);
                    HandleBossDefeated();
                }

                //エリアを無効にする
                missionAreaInstance.SetActive(false);

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

        var spawnedEnemy = InstantiateEnemy(_enemydata, _position);
        enemyPool.Add(spawnedEnemy);
        totalEnemyCount++;

        //ボスの場合、通知を送信
        if (_enemydata.enemyRank == EnemyRank.Boss)
        {
            CustomLogger.Log($"ボスがスポーンしました: {spawnedEnemy.name}");
            BossSpawnNotifier.Instance.NotifyBossSpawned(spawnedEnemy);
        }
    }

    private GameObject InstantiateEnemy(EnemyStatusData _data, Vector3 _position)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Enemy/EnemyContainer"), _position, Quaternion.identity, enemyCollector.transform);
        obj.GetComponent<EnemyController>().Status.StatusData = _data;
        obj.name = _data.name;
        obj.GetComponent<EnemyController>().ParentSpawner = this.gameObject;
        return obj;
    }

    /// <summary>
    /// ボスが倒されたときに呼び出されるコールバック
    /// 仮
    /// </summary>
    public void HandleBossDefeated()
    {


        GameObject resultObj = Instantiate(resultPrefab, transform.position, Quaternion.identity);
        _resultScreenShot = resultObj.GetComponent<ResultScreenShot>();

        if (_resultScreenShot == null)
        {
            UnityEngine.Debug.LogError("ResultScreenShot component not found on prefab!");
            return;
        }

        _resultScreenShot.OnScreenshotCompleted += OnScreenshotCompleted;
        _resultScreenShot.StartScreenshotProcess();


        MissionResultManager.Instance.NotifyMissionComplete(MissionType.KillBoss);
        UnityEngine.Debug.Log("ボスを倒しました。");
    }

    /// <summary>
    /// スクリーンショットが完了したときに呼び出されるコールバック
    /// 仮
    /// </summary>
    private void OnScreenshotCompleted()
    {
        UnityEngine.Debug.Log("Screenshot process completed");
        if (_resultScreenShot != null)
        {
            _resultScreenShot.OnScreenshotCompleted -= OnScreenshotCompleted;
        }
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
        //表示更新
        UpdateText();
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