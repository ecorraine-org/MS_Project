using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChicken : EnemyAction
{
    public override void Move()
    {
        // 逃げる
        if (distanceToPlayer < enemy.Status.StatusData.attackDistance)
        {
            Vector3 newMovement = -transform.forward * 0.88f * Time.deltaTime;
            enemy.RigidBody.MovePosition(enemy.RigidBody.position + newMovement);
        }
    }
    public override void Attack()
    {
    }
}
