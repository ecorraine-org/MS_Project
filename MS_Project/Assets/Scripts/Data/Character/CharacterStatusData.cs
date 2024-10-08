using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/Character/CharacterStatusData", order = 0)]
public class CharacterStatusData : ScriptableObject
{
    [Header("HP")]
    public float maxHealth = 100.0f;

    [Header("移動速度")]
    public float velocity = 1.0f;
}
