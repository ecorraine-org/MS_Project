using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class EnemyStateIdle : EnemyState
{
    private MethodInfo idleTickMethod;

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        idleTickMethod = enemy.EnemyAction.GetType().GetMethod("IdleTick");

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
        enemy.Anim.Play("Idle");
    }

    public override void Tick()
    {
        base.Tick();

        //敵による独自の処理
        if (idleTickMethod != null)
        {
            idleTickMethod.Invoke(enemy.EnemyAction, null);
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
        //死亡チェック
        if (enemyStateHandler.CheckDeath()) return;

        //ダメージチェック
        if (enemyStateHandler.CheckHit()) return;

        //スキルへ遷移
        if (enemyStateHandler.CheckSkill()) return;

        //移動へ遷移
        float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distanceToPlayer <= enemyStatusHandler.StatusData.chaseDistance)
        {
            enemy.State.TransitionState(ObjectStateType.Walk);
            return;
        }

        //攻撃へ遷移
        //if (enemyStateHandler.CheckAttack()) return;
        if (distanceToPlayer <= enemyStatusHandler.StatusData.attackDistance && enemy.AllowAttack)
        {
            //クールダウン
            enemy.StartAttackCoroutine();

            enemy.State.TransitionState(ObjectStateType.Attack);
            return;
        }
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
