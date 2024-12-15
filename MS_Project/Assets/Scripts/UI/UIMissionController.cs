using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIMissionController : MonoBehaviour
{
    [SerializeField, Header("ミッションＵＩ")]
    private GameObject missionTitle;
    [SerializeField]
    private GameObject missionPanel;

    [SerializeField, Header("エネミーミッション")]
    private GameObject enemyMissionItem;

    [SerializeField, Header("ボスミッション")]
    private GameObject bossMissionItem;

    [SerializeField, Header("オノマトペミッション")]
    private GameObject onomatoMissionItem;

    [SerializeField, Header("倒したエネミー数")]
    private Text enemyTxt;

    [SerializeField, Header("倒したボス数")]
    private Text bossTxt;

    [SerializeField, Header("集めたオノマトペ数")]
    private Text onomatoTxt;

    [SerializeField, Header("ミッションタイプ"), Tooltip("ミッションタイプ")]
    private MissionType missionType = MissionType.None;

    private PlayerController player;
    private EnemySpawner spawner;
    private ObjectCollector collector;

    private void Awake()
    {
        /*
        missionTitle = this.transform.GetChild(0).gameObject;
        missionItem = this.transform.GetChild(1).gameObject;
        */
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject.GetComponent<ObjectCollector>();
    }

    private void Start()
    {
        for (int child = 0; child < this.transform.childCount; child++)
        {
            if (this.transform.GetChild(child).gameObject.activeInHierarchy)
                this.transform.GetChild(child).gameObject.SetActive(false);
        }

        //子オブジェクト含めて、有効にする
        missionPanel.SetActive(true);
        // 一旦子オブジェクトを全部無効にして、条件によって有効にする
        enemyMissionItem.SetActive(false);
        bossMissionItem.SetActive(false);
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
                missionTitle.GetComponentInChildren<TextMeshProUGUI>().text = "<b>敵殲滅</b>";
                missionText = "　　敵を倒せ　" + _string;
                break;
            case MissionType.KillBoss:
                missionTitle.GetComponentInChildren<TextMeshProUGUI>().text = "<b>ボス撃破</b>";
                missionText = "　　ボス　" + _string;
                break;
            case MissionType.OpenRoute:
                missionTitle.GetComponentInChildren<TextMeshProUGUI>().text = "<b>？？？</b>";
                missionText = "？？？";
                break;
            case MissionType.Protect:
                missionTitle.GetComponentInChildren<TextMeshProUGUI>().text = "<b>？？？</b>";
                missionText = "？？？";
                break;

            case MissionType.Tutorial:
                missionTitle.GetComponentInChildren<TextMeshProUGUI>().text = "<b>チュートリアル</b>";
                missionText = " ";
                break;
            default:
                missionText = " ";
                break;
                
        }

        missionTitle.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
        missionTitle.GetComponentInChildren<TextMeshProUGUI>().outlineWidth = 0.2f;
        missionTitle.GetComponentInChildren<TextMeshProUGUI>().outlineColor = new Color32(255, 255, 255, 255);


        // return tmpMission.text = missionText;
        if (tmpMission) tmpMission.text = missionText;
        return missionText;
    }

    #region　Getter and Setter
    public GameObject MissionTitle
    {
        get => missionTitle;
    }

    public GameObject MissionPanel
    {
        get => missionPanel;
    }

    public GameObject EnemyMissionItem
    {
        get => enemyMissionItem;
    }

    public GameObject BossMissionItem
    {
        get => bossMissionItem;
    }

    public GameObject OnomatoMissionItem
    {
        get => onomatoMissionItem;
    }

    public Text EnemyTxt
    {
        get => enemyTxt;
        set { enemyTxt = value; }
    }

    public Text BossTxt
    {
        get => bossTxt;
        set { bossTxt = value; }
    }

    public Text OnomatoTxt
    {
        get => onomatoTxt;
        set { onomatoTxt = value; }
    }

    public TextMeshProUGUI tmpMission
    {
        get => MissionPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    public EnemySpawner Spawner
    {
        get => spawner;
        set { spawner = value; }
    }
    #endregion
}
