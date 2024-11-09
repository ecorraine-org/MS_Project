using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class StateAttack : ObjectState
{
    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);
    }

    public override void Tick()
    {
        if (enemy != null)
        {
            if(!enemy.CanAttack)
                enemy.State.TransitionState(ObjectStateType.Idle);

            enemy.OnAttack?.Invoke();
        }
        else
        {
            // ダメージチェック
            if (objStateHandler.CheckHit()) return;

            // スキルへ遷移
            if (objStateHandler.CheckSkill()) return;

            // アイドルへ遷移
            if (objController.MovementInput.magnitude <= 0f && !objController.CanAttack)
                objStateHandler.TransitionState(ObjectStateType.Idle);
        }
    }

    public override void FixedTick()
    {
    }

    public override void Exit()
    {
    }
}
