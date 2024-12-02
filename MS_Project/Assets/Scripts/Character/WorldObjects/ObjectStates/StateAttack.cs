using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class StateAttack : ObjectState
{
    private MethodInfo attackTickMethod;
    public LayerMask targetLayer;

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
            if (enemy != null) enemy.Anim.SetTrigger("IsAttack");
        }
    }

    public override void Tick()
    {
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
        if (enemy != null)
        {
            enemy.AttackCollider.CanHit = true;
            enemy.AttackCollider.DetectColliders(enemy.EnemyStatus.StatusData.damage, targetLayer, false);
        }

        if (objStateHandler.CheckDeath()) return;

        //ダメージチェック
        if (objStateHandler.CheckHit()) return;

        //スキルへ遷移
        if (objStateHandler.CheckSkill()) return;

        //アイドルへ遷移
        /*
        if (objController.MovementInput.magnitude <= 0f && !objController.IsAttacking)
            objStateHandler.TransitionState(ObjectStateType.Idle);
        */
        if (enemy.AnimManager != null && enemy.AnimManager.IsAnimEnd)
        {
            objController.State.TransitionState(ObjectStateType.Idle);
        }
    }

    public override void FixedTick()
    {
    }

    public override void Exit()
    {
        objController.AttackCollider.CanHit = false;
    }
}
