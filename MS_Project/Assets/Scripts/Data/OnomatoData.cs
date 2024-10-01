using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Onomatopoeia", order = 0)]
public class OnomatoData : ScriptableObject
{
    [Header("オノマトペタイプ"), Tooltip("生成するオノマトペ")]
    public OnomatoType type = OnomatoType.None;
}
