using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトのステータスを管理するビヘイビア
/// </summary>
public class ObjectStatusHandler : StatusManager
{
    [Tooltip("攻撃されたか？")]
    private bool isDamaged = false;

    [Tooltip("生きているか？")]
    private bool isAlive = true;

    [SerializeField, Tooltip("行動クールタイム")]
    private float actionCooldown;

    [SerializeField, Tooltip("自分の属性")]
    private OnomatoType selfType;

    [SerializeField, Tooltip("オノマトペデータ")]
    private OnomatopoeiaData onomatoData;

    [SerializeField, Tooltip("耐性")]
    private OnomatoType tolerance;


    ObjectStatusData objectStatusData;

    protected override void Awake()
    {
        //base.Awake();
    }

    protected virtual void Start()
    {
        if (!StatusData)
        {
            CustomLogger.Log("No status data found. Instantiating from new.");
            StatusData = (ObjectStatusData)base.StatusData;
        }
        else
        {
            CustomLogger.Log("Found " + StatusData.ToString() + "\nInstantiating...");
        }

        statusData = objectStatusData;
        currentHealth = objectStatusData.maxHealth;
        isInvincible = objectStatusData.isInvincible;
        actionCooldown = objectStatusData.timeTillNextAction;
        selfType = objectStatusData.SelfType;
        onomatoData = objectStatusData.onomatoData;
        tolerance = objectStatusData.tolerance;
    }

    public new ObjectStatusData StatusData
    {
        get => (ObjectStatusData)objectStatusData;
        set { objectStatusData = value; }
    }

    #region Getter & Setter

    public float ActionCooldown
    {
        get => actionCooldown;
        set { actionCooldown = value; }
    }

    public bool IsDamaged
    {
        get => isDamaged;
        set { isDamaged = value; }
    }

    public bool IsAlive
    {
        get => isAlive;
        set { isAlive = value; }
    }

    #endregion
}
