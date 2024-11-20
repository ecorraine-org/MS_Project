using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatusData", menuName = "ScriptableObjects/Player/PlayerStatusData", order = 1)]
public class PlayerStatusData : BaseStatusData
{
    [Header("移動速度")]
    public float velocity = 1.0f;

    [Header("暴走ゲージ最大値")]
    public float maxFrenzyGauge = 20.0f;

    [Header("暴走時間(秒)")]
    public float frenzyTime = 5.0f;

    [Header("剣の攻撃力")]
    public float swordAtk = 1.0f;

    [Header("ハンマーの攻撃力")]
    public float hammerAtk = 1.0f;

    [Header("スピアの攻撃力")]
    public float spearAtk = 1.0f;

    [Header("メリケンサックの攻撃力")]
    public float gauntletAtk = 1.0f;

    [Header("攻撃オノマトペリスト")]
    public List<OnomatopoeiaData> onomatoAttackData = new List<OnomatopoeiaData>();

    [Header("空振り攻撃オノマトペリスト")]
    public List<OnomatopoeiaData> onomatoAttackMissData = new List<OnomatopoeiaData>();

    private void OnEnable()
    {
        ObjectType = WorldObjectType.Player;
        CustomLogger.Log("Playerオブジェクトタイプを初期化");
    }
}
