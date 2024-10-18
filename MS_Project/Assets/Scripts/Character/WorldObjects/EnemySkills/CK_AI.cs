using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CK_AI : EnemySkill
{
    //前にツッコむ
    public override void SkillAttack()
    {
        // 俺はもうね。逃げる
        {
            Vector3 movement = -transform.forward * 0.88f * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }
}
