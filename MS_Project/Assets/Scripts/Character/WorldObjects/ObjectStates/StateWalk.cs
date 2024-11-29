using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateWalk : ObjectState
{
    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

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
        if (enemy != null)
        {
            enemy.Anim.Play("Walk");
        }
    }

    public override void Tick()
    {
        //敵による独自の処理
        var method = enemy.EnemyAction.GetType().GetMethod("WalkTick");
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
        Vector3 direction = player.transform.position - enemy.transform.position;
        //進む方向に向く
        Quaternion forwardRotation = Quaternion.LookRotation(direction.normalized);
        forwardRotation.x = 0f;
        enemy.transform.rotation = forwardRotation;

        //追跡
        enemy.OnMovementInput?.Invoke(direction.normalized);

        //移動
        enemy.Move();

        if (objStateHandler.CheckDeath()) return;

        //ダメージチェック
        if (objStateHandler.CheckHit()) return;

        //スキルへ遷移
        if (objStateHandler.CheckSkill()) return;

        //アイドルへ遷移
        /*
        if (objController.MovementInput.magnitude <= 0f)
        {
            objController.State.TransitionState(ObjectStateType.Idle);
        };
        */
        float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distanceToPlayer > enemyStatusHandler.StatusData.chaseDistance)
        {
            objController.State.TransitionState(ObjectStateType.Idle);
            return;
        }

        //攻撃へ遷移
        // if (objStateHandler.CheckAttack()) return;
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
        //敵による独自の処理
        var method = enemy.EnemyAction.GetType().GetMethod("WalkExit");
        if (method != null)
        {
            method.Invoke(enemy.EnemyAction, null);
        }
    }
}
