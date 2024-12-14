using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageOnomatoData", menuName = "ScriptableObjects/Onomatopoeia/StageOnomatoData", order = 0)]
public class StageOnomatoData : ScriptableObject
{
    [Header("リスト")]
    public List<OnomatopoeiaData> StageOnomatoList = new List<OnomatopoeiaData>();



}
