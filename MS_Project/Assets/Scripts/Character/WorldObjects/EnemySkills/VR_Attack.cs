using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Attack : EnemySkill
{
    //前にツッコむ
    public override void SkillAttack()
    {
        Vector3 movement = transform.forward * 2.0f * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
