using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CK_AI : EnemyAction
{
    public override void Move()
    {
        // 俺はもうね。逃げる
        {
            Vector3 movement = -transform.forward * 0.88f * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }
    public override void SkillAttack()
    {
    }
}
