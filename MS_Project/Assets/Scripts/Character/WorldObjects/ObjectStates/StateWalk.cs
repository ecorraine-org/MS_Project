using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWalk : ObjectState
{
    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        if (enemy != null)
        {
            enemy.Anim.Play("Walk");
        }
    }

    public override void Tick()
    {
        // ダメージチェック
        if (objStateHandler.CheckHit()) return;

        // 攻撃へ遷移
        if (objStateHandler.CheckAttack()) return;

        // スキルへ遷移
        if (objStateHandler.CheckSkill()) return;

        // アイドルへ遷移
        if (objController.MovementInput.magnitude <= 0f)
        {
            objController.State.TransitionState(ObjectStateType.Idle);
        };
    }

    public override void FixedTick()
    {
    }

    public override void Exit()
    {
    }
}
