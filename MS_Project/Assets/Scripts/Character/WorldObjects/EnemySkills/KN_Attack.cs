using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KN_Attack : EnemyAction
{
    public override void SkillAttack()
    {

    }

    //確率で防御
    public void Guard()
    {
        int rnd = Random.Range(1, 10);
        if (rnd >= 5)
        {
            animator.Play("Guard", 0, 0f);
        }
    }
}