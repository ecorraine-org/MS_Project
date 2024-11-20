using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIMissionController : MonoBehaviour
{
    private GameObject missionItem;

    private PlayerController player;
    private EnemySpawner spawner;
    private Collector collector;

    private void Awake()
    {
        missionItem = this.transform.GetChild(0).gameObject;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject.GetComponent<Collector>();
    }

    private void Start()
    {
        if (missionItem.activeInHierarchy)
            missionItem.SetActive(false);
    }

    #region
    public GameObject MissionItem
    {
        get => missionItem;
    }

    public string MissionText
    {
        get => missionItem.GetComponentInChildren<TextMeshProUGUI>().text;
        set { missionItem.GetComponentInChildren<TextMeshProUGUI>().text = value; }
    }

    public EnemySpawner Spawner
    {
        get => spawner;
        set { spawner = value; }
    }
    #endregion
}
