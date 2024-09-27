using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Character/CharaStatusData", order = 0)]
public class CharaStatusData : ScriptableObject
{
    [Header("HP")]
    public float maxHealth = 100.0f;

    [Header("ˆÚ“®‘¬“x")]
    public float velocity = 1.0f;
}
