using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "OnomatoData", menuName = "ScriptableObjects/Onomatopoeia/OnomatopoeiaData", order = 0)]
public class OnomatopoeiaData : ScriptableObject
{
    [Header("生成情報")]
    [Header("オノマトペ名")]
    public string wordToUse;

    [Header("フォント")]
    public TMP_FontAsset fontAsset;

    [Header("色")]
    public UnityEngine.Color32 fontColor = new Color32(0, 0, 0, 255);

    [Header("オノマトペ種類")]
    public OnomatoType type = OnomatoType.None;

    [Header("ステータス加算：")]
    [Header("ダメージ")]
    public float damageBuff = 0.0f;

    [Header("速度")]
    public float speedBuff = 0.0f;

    [Header("回復")]
    public float healBuff = 0.0f;
}
