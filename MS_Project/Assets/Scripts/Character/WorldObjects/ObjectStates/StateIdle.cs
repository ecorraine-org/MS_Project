using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : ObjectState
{
    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        if (enemy != null && !enemy.IsAttacking)
            enemy.Anim.Play("Idle");
    }

    public override void Tick()
    {
        if (objStateHandler.CheckDeath()) return;

        //ダメージチェック
        if (objStateHandler.CheckHit()) return;

        //スキルへ遷移
        if (objStateHandler.CheckSkill()) return;

        //移動へ遷移
        /*
        if (objController.MovementInput.magnitude > 0.1f)
        {
            objController.State.TransitionState(ObjectStateType.Walk);
        };
        */
        float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distanceToPlayer <= enemyStatusHandler.StatusData.chaseDistance)
        {
            objController.State.TransitionState(ObjectStateType.Walk);
            return;
        }

        //攻撃へ遷移
        //if (objStateHandler.CheckAttack()) return;
        if (distanceToPlayer <= enemyStatusHandler.StatusData.attackDistance && enemy.AllowAttack)
        {
            //クールダウン
            enemy.StartAttackCoroutine();

            objController.State.TransitionState(ObjectStateType.Attack);
            return;
        }
    }

    public override void FixedTick()
    {
    }

    public override void Exit()
    {
    }
}
