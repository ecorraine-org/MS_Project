using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/Character/CharacterStatusData", order = 0)]
public class CharacterStatusData : ScriptableObject
{
    [SerializeField, Header("オブジェクトタイプ")]
    const WorldObjectType objectType = WorldObjectType.None;

    [Header("最大HP")]
    public float maxHealth = 100.0f;

    public WorldObjectType ObjectType
    {
        get => objectType;
    }
}
