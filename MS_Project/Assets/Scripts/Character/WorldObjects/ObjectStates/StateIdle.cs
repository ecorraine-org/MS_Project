using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : ObjectState
{
    public override void Init(ObjectController _objectController)
    {
        base.Init(_objectController);
    }

    public override void Tick()
    {
        if (enemy != null)
        {
            enemy.Anim.Play("Idle");
        }

        // ダメージチェック
        if (objStateHandler.CheckHit()) return;

        // 攻撃へ遷移
        if (objStateHandler.CheckAttack()) return;

        // スキルへ遷移
        if (objStateHandler.CheckSkill()) return;

        // 移動へ遷移
        if (objController.MovementInput.magnitude > 0.1f)
        {
            objController.State.TransitionState(ObjectStateType.Walk);
        };
    }

    public override void FixedTick()
    {
    }

    public override void Exit()
    {
    }
}
