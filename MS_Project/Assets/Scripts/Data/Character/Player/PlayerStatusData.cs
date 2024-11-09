using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatusData", menuName = "ScriptableObjects/Player/PlayerStatusData", order = 1)]
public class PlayerStatusData : BaseStatusData
{
    [Header("オブジェクトタイプ")]
    public WorldObjectType objectType = WorldObjectType.Player;

    [Header("移動速度")]
    public float velocity = 1.0f;

    [Header("暴走ゲージ最大値")]
    public float maxFrenzyGauge = 20;

    [Header("暴走時間(秒)")]
    public float frenzyTime = 5;

    [Header("剣の攻撃力")]
    public float swordAtk = 1;

    [Header("ハンマーの攻撃力")]
    public float hammerAtk = 1;

    [Header("スピアの攻撃力")]
    public float spearAtk = 1;

    [Header("メリケンサックの攻撃力")]
    public float gauntletAtk = 1;
}
