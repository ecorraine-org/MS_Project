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
