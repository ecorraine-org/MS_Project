using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "ScriptableObjects/WorldObjects/ObjectStatusData", order = 1)]
public class ObjectStatusData : CharacterStatusData
{
    [Header("オブジェクトタイプ")]
    public readonly WorldObjectType objectType = WorldObjectType.StaticObject;

    [Header("オブジェクトプレハブ")]
    public string gameObjPrefab;

    [Header("エネミータイプ")]
    public OnomatoType SelfType = OnomatoType.None;

    [Header("オノマトペデータ")]
    public OnomatopoeiaData onomatoData;

    [Header("耐性")]
    public OnomatoType tolerance = OnomatoType.None;

    [Header("SE")]
    public string sfxClip;

    [Header("VFX")]
    public string vfxClip;

    [Header("行動クールタイム")]
    public float timeTillNextAction = 1f;
}
