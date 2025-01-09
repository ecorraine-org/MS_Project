using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームパラメーター管理
/// </summary>
public class GameStatsManager : SingletonBaseBehavior<GameStatsManager>
{

    [SerializeField, Header("ゲーム時間")]
    public float gameTime;


    protected override void AwakeProcess()
    {

    }

    private void Update()
    {
        gameTime += Time.deltaTime;
    }
}
