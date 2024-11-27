using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class OnomatoDataBase : ScriptableObject
{
    [SerializeField, Header("オノマトペオブジェクト管理用リスト")]
    public List<OnomatopoeiaData> OnomatoEnemyList = new List<OnomatopoeiaData>();
    public List<OnomatopoeiaData> OnomatoPlayerList = new List<OnomatopoeiaData>();
    public List<OnomatopoeiaData> OnomatoObjectList = new List<OnomatopoeiaData>();
}