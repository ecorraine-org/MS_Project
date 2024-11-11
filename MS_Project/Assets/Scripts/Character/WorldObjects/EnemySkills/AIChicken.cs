using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChicken : EnemyAction
{
    protected EnemyController enemy;
    private EnemyAction enemyAction;

    private void Update()
    {
        // 逃げる
       // if (enemy != null && enemy.MovementInput.magnitude > 0.1f && enemy.EnemyStatus.MoveSpeed > 0)
            enemy.RigidBody.velocity = enemy.MovementInput * enemy.EnemyStatus.MoveSpeed;
        Debug.Log("upsareteruuuuuu");
    }

    public override void Move()
    {
        // 逃げる
        if (distanceToPlayer < enemy.EnemyStatus.StatusData.attackDistance)
        {
            Vector3 newMovement = -transform.forward * 0.88f * Time.deltaTime;
            enemy.RigidBody.MovePosition(enemy.RigidBody.position + newMovement);
        }
    }
    public override void Attack()
    {
    }
}
