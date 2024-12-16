using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class EnemyStateAttack : EnemyState
{
    private MethodInfo attackTickMethod;

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        attackTickMethod = enemy.EnemyAction.GetType().GetMethod("AttackTick");

        //敵による独自の処理
        var method = enemy.EnemyAction.GetType().GetMethod("AttackInit");
        if (method != null)
        {
            method.Invoke(enemy.EnemyAction, null);
        }
        else
        {
            enemy.Anim.SetTrigger("IsAttack");
        }
    }

    public override void Tick()
    {
        base.Tick();

        //敵による独自の処理
        if (attackTickMethod != null)
        {
            attackTickMethod.Invoke(enemy.EnemyAction, null);
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
        //enemy.AttackCollider.CanHit = true;
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        if (enemyStateHandler.CheckDeath()) return;

        //ダメージチェック
        if (enemyStateHandler.CheckHit()) return;

        //スキルへ遷移
        if (enemyStateHandler.CheckSkill()) return;

        //アイドルへ遷移
        /*
        if (objController.MovementInput.magnitude <= 0f && !objController.IsAttacking)
            enemyStateHandler.TransitionState(ObjectStateType.Idle);
        */
        if (enemy.AnimManager != null && enemy.AnimManager.IsAnimEnd)
        {
            enemy.State.TransitionState(ObjectStateType.Idle);
        }
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.AttackCollider.CanHit = false;
    }
}
