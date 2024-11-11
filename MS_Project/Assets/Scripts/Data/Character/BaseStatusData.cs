using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStatusData", menuName = "ScriptableObjects/BaseStatusData", order = 0)]
public class BaseStatusData : ScriptableObject
{
    [Header("オブジェクトタイプ")]
    WorldObjectType objectType = WorldObjectType.None;

    [Header("無敵かどうか")]
    public bool isInvincible = false;

    [Header("最大HP")]
    public float maxHealth = 100.0f;

    public WorldObjectType ObjectType
    {
        get => objectType;
        set { objectType = value; }
    }
}