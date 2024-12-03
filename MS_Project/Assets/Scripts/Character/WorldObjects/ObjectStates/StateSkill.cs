using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSkill : ObjectState
{
    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        //敵による独自の処理
        var method = enemy.EnemyAction.GetType().GetMethod("SkillInit");
        if (method != null)
        {
            method.Invoke(enemy.EnemyAction, null);
        }
    }

    public override void Tick()
    {

        //敵による独自の処理
        var method = enemy.EnemyAction.GetType().GetMethod("SkillTick");
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
        if (enemy != null) return;

        AnimatorStateInfo stateInfo = enemy.Anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 1f)
        {
            enemy.State.TransitionState(ObjectStateType.Idle);
        }

        // ダメージチェック
        if (objStateHandler.CheckHit()) return;

    }

    public override void FixedTick()
    {
    }

    public override void Exit()
    {
    }
}
