using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnomatoData", menuName = "ScriptableObjects/Onomatopoeia/OnomatopoeiaData", order = 0)]
public class OnomatopoeiaData : ScriptableObject
{
    [Header("生成情報")]
    [Tooltip("オノマトペ名")]
    public string word;

    [Tooltip("オノマトペ種類")]
    public OnomatoType type = OnomatoType.None;

    [Header("ステータス加算")]
    [Tooltip("ダメージ")]
    public int iDamage = 0;

    [Tooltip("速度")]
    public float fSpeed = 0f;
}
