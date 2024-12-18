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

    [Header("画像")]
    public Sprite onomatoSprite;

    [Header("画像サイズ")]
    public float spriteSize = 1.0f;

    [Header("アニメーション速度")]
    public float spriteAnimSpeed = 1.0f;

    [Header("アニメーションコントローラー")]
    public AnimatorOverrideController onomatoACont;

    [Header("フォント")]
    public TMP_FontAsset fontAsset;

    [Header("色")]
    public UnityEngine.Color32 fontColor = new Color32(0, 0, 0, 255);

    [Header("オノマトペ種類")]
    public OnomatoType type = OnomatoType.None;

    [Header("ステータス乗算する倍率：")]
    [Header("ダメージ")]
    public float damageBuff = 0.0f;

    [Header("速度")]
    public float speedBuff = 0.0f;

    [Header("回復量(加算)")]
    public float healBuff = 0.0f;

    [Header("バフ持続時間(秒)")]
    public float buffDuration = 10.0f;

    [Header("SE")]
    public AudioClip onomatoSE;


}
