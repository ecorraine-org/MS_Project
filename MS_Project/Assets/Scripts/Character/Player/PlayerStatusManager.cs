using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステータスを管理するビヘイビア
/// </summary>
public class PlayerStatusManager : StatusManager
{
    //PlayerControllerの参照
    PlayerController playerController;

    PlayerStatusData playerStatusData;

    //暴走
    //****************
    [SerializeField, Header("暴走値")]
    float frenzyValue = 0;

    //デフォルトのサイズ
    UnityEngine.Vector3 defaultSize;

    float frenzyTimer = 0;

 

    //暴走しているか
    bool isFrenzy = false;
    //****************

    protected override void Awake()
    {
        base.Awake();

        playerStatusData = ScriptableObject.CreateInstance<PlayerStatusData>(); 
        
    }

    private void OnEnable()
    {
        //イベントをバインドする
        OnomatoManager.OnIncreaseFrenzyEvent += IncreaseFrenzy;
    }

    private void OnDisable()
    {
        //バインドを解除する
        OnomatoManager.OnIncreaseFrenzyEvent -= IncreaseFrenzy;
    }

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;

        defaultSize = playerController.transform.localScale;
    }

    private void Update()
    {
        //暴走の仮処理
        if (frenzyValue >= playerStatusData.maxFrenzyGauge)
        {
            frenzyValue = 0;
            playerController.transform.localScale = 2 * defaultSize;

            frenzyTimer = playerStatusData.frenzyTime;
            isFrenzy = true;
        }

        if (frenzyTimer > 0 && isFrenzy)
        {
            frenzyTimer -= Time.deltaTime;
        }

        //暴走終了
        if (frenzyTimer <= 0 && isFrenzy)
        {
            playerController.transform.localScale = defaultSize;
            isFrenzy = false;
        }
    }

    /// <summary>
    /// 暴走ゲージを溜める
    /// </summary>
    private void IncreaseFrenzy(float _amount)
    {
        frenzyValue += _amount;
    }

    public new PlayerStatusData StatusData
    {
        get => playerStatusData;
    }

    public float FrenzyValue
    {
        get => frenzyValue;
    }
}
