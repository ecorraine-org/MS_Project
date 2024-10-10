using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkillData", menuName = "ScriptableObjects/Character/Player/SkillData", order = 0)]
public class PlayerSkillData : ScriptableObject
{
    [Header("捕食クールタイム")]
    public float eatingCoolTime = 5.0f;

    [Header("剣スキルクールタイム")]
    public float swordSkillCoolTime = 7.0f;

    [Header("ハンマースキルクールタイム")]
    public float hammerSkillCoolTime = 5.0f;
}
