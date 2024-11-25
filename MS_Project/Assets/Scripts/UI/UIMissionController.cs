using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIMissionController : MonoBehaviour
{
    [SerializeField, Header("ミッションタイプ"), Tooltip("ミッションタイプ")]
    private MissionType missionType = MissionType.None;

    private GameObject missionTitle;
    private GameObject missionItem;

    private PlayerController player;
    private EnemySpawner spawner;
    private Collector collector;

    private void Awake()
    {
        missionTitle = this.transform.GetChild(0).gameObject;
        missionItem = this.transform.GetChild(1).gameObject;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject.GetComponent<Collector>();
    }

    private void Start()
    {
        for (int child = 0; child < this.transform.childCount; child++)
        {
            if (this.transform.GetChild(child).gameObject.activeInHierarchy)
                this.transform.GetChild(child).gameObject.SetActive(false);
        }
    }

    private void CheckMission(MissionType _missiontype)
    {
        switch (_missiontype)
        {
            case MissionType.KillAll:
                // Handle KillAll mission type
                break;
            case MissionType.KillBoss:
                // Handle KillBoss mission type
                break;
            case MissionType.OpenRoute:
                // Handle OpenRoute mission type
                break;
            case MissionType.Protect:
                // Handle Protect mission type
                break;
        }
    }

    public string GetMissionDetails(MissionType _missiontype, string _string)
    {
        string missionText = "";

        switch (_missiontype)
        {
            case MissionType.KillAll:
                missionTitle.GetComponentInChildren<Text>().text = "<b>敵殲滅</b>";
                missionText = "　　敵を倒せ　" + _string;
                break;
            case MissionType.KillBoss:
                missionTitle.GetComponentInChildren<Text>().text = "<b>ボス撃破</b>";
                missionText = "　　ボス　" + _string;
                break;
            case MissionType.OpenRoute:
                missionTitle.GetComponentInChildren<Text>().text = "<b>？？？</b>";
                missionText = "？？？";
                break;
            case MissionType.Protect:
                missionTitle.GetComponentInChildren<Text>().text = "<b>？？？</b>";
                missionText = "？？？";
                break;
        }

        return tmpMission.text = missionText;
    }

    #region　Getter and Setter
    public GameObject MissionTitle
    {
        get => missionTitle;
    }

    public GameObject MissionItem
    {
        get => missionItem;
    }

    public TextMeshProUGUI tmpMission
    {
        get => MissionItem.GetComponentInChildren<TextMeshProUGUI>();
    }

    public EnemySpawner Spawner
    {
        get => spawner;
        set { spawner = value; }
    }
    #endregion
}
