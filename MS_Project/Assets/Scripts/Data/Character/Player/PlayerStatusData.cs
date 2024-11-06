using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatusData", menuName = "ScriptableObjects/Character/Player/StatusData", order = 1)]
public class PlayerStatusData : CharacterStatusData
{
    [Header("移動速度")]
    public float velocity = 1.0f;

    [Header("暴走ゲージ最大値")]
    public float maxFrenzyGauge = 20;

    [Header("暴走時間(秒)")]
    public float frenzyTime = 5;
}
