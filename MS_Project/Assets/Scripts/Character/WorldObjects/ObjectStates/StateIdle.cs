using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : ObjectState
{
    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        //敵による独自の処理
        var method = enemy.EnemyAction.GetType().GetMethod("IdleInit");
        if (method != null)
        {
            method.Invoke(enemy.EnemyAction, null);
        }
        else
        {
            normalInit();
        }
    }

    /// <summary>
    /// 共通の処理
    /// </summary>
    private void normalInit()
    {
        if (enemy != null)
            enemy.Anim.Play("Idle");
    }

    public override void Tick()
    {
        //敵による独自の処理
        var method = enemy.EnemyAction.GetType().GetMethod("IdleTick");
        if (method != null)
        {
            method.Invoke(enemy.EnemyAction, null);
        }
        else
        {
            normalTick();
        }
    }

    /// <summary>
    /// 共通の処理
    /// </summary>
    private void normalTick() 
    {
        if (objStateHandler.CheckDeath()) return;

        //ダメージチェック
        if (objStateHandler.CheckHit()) return;

        //スキルへ遷移
        if (objStateHandler.CheckSkill()) return;

        //移動へ遷移
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
