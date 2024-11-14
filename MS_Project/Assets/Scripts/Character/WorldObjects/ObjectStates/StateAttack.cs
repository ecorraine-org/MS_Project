using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class StateAttack : ObjectState
{
    public LayerMask targetLayer;

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        if (enemy != null)
        {
            enemy.Anim.SetTrigger("IsAttack");
        }
    }

    public override void Tick()
    {
        if (enemy != null)
        {
            enemy.AttackCollider.CanHit = true;
            enemy.AttackCollider.DetectColliders(20, targetLayer, false);
            Debug.Log("attackColliderManager");
        }

        // ダメージチェック
        if (objStateHandler.CheckHit()) return;

        // スキルへ遷移
        if (objStateHandler.CheckSkill()) return;

        // アイドルへ遷移
        if (objController.MovementInput.magnitude <= 0f /*&& !objController.IsAttacking*/)
            objStateHandler.TransitionState(ObjectStateType.Idle);
    }

    public override void FixedTick()
    {
    }

    public override void Exit()
    {
        objController.AttackCollider.CanHit = false;
    }
}
