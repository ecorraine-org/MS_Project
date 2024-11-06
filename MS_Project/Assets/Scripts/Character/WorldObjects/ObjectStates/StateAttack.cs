using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : ObjectState
{
    public override void Init(ObjectController _objectController)
    {
        base.Init(_objectController);
    }

    public override void Tick()
    {
        if (objController.Status.StatusData.ObjectType == WorldObjectType.Enemy)
        {
            EnemyController enemy = objController as EnemyController;
            AnimatorStateInfo stateInfo = enemy.Anim.GetCurrentAnimatorStateInfo(0);

            // ダメージチェック
            if (enemy.State.CheckHit()) return;

            // スキルへ遷移
            if (objStateHandler.CheckSkill()) return;

            // アイドルへ遷移
            if (enemy.MovementInput.magnitude < 0f)
            {
                enemy.State.TransitionState(ObjectStateType.Idle);
            };
        }
        else
            return;
    }

    public override void FixedTick()
    {
    }

    public override void Exit()
    {
    }
}
