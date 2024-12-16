using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class EnemyStateWalk : EnemyState
{
    private MethodInfo walkTickMethod;

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        walkTickMethod = enemy.EnemyAction.GetType().GetMethod("WalkTick");

        //敵による独自の処理
        var method = enemy.EnemyAction.GetType().GetMethod("WalkInit");
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
        enemy.Anim.Play("Walk");
    }

    public override void Tick()
    {
        base.Tick();

        //敵による独自の処理
        if (walkTickMethod != null)
        {
            walkTickMethod.Invoke(enemy.EnemyAction, null);
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
        Vector3 direction = player.transform.position - enemy.transform.position;
        //進む方向に向く
        Quaternion forwardRotation = Quaternion.LookRotation(direction.normalized);
        forwardRotation.x = 0f;
        forwardRotation.z = 0f;
        enemy.transform.rotation = forwardRotation;

        //追跡
        enemy.OnMovementInput?.Invoke(direction.normalized);

        //移動
        enemy.Move();

        if (enemyStateHandler.CheckDeath()) return;

        //ダメージチェック
        if (enemyStateHandler.CheckHit()) return;

        //スキルへ遷移
        if (enemyStateHandler.CheckSkill()) return;

        //アイドルへ遷移
        float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distanceToPlayer > enemyStatusHandler.StatusData.chaseDistance)
        {
            enemy.State.TransitionState(ObjectStateType.Idle);
            return;
        }

        //攻撃へ遷移
        // if (enemyStateHandler.CheckAttack()) return;
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

        //敵による独自の処理
        var method = enemy.EnemyAction.GetType().GetMethod("WalkExit");
        if (method != null)
        {
            method.Invoke(enemy.EnemyAction, null);
        }
    }
}
