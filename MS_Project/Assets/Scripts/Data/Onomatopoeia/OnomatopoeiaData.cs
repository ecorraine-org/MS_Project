using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnomatoData", menuName = "ScriptableObjects/Onomatopoeia/OnomatopoeiaData", order = 0)]
public class OnomatopoeiaData : ScriptableObject
{
    [Header("生成情報")]
    [Header("オノマトペ名")]
    public string wordToUse;

    [Header("オノマトペ種類")]
    public OnomatoType type = OnomatoType.None;

    [Header("ステータス加算：")]
    [Header("ダメージ")]
    public float damageBuff = 0;

    [Header("速度")]
    public float speedBuff = 0f;

    [Header("回復")]
    public float healBuff = 0f;
}
