using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionTemplate : EnemyAction
{
    public override void SkillAttack()
    {
        Debug.Log("UseSkill");
    }
}