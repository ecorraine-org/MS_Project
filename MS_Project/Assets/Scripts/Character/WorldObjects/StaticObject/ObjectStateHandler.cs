using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateHandler : WorldObjectStateHandler
{
    ObjectController staticObj;

    [SerializeField, Header("アイドル状態ビヘイビア")]
    ObjectStateIdle idleState;

    [SerializeField, Header("攻撃状態ビヘイビア")]
    ObjectStateAttack attackState;

    [SerializeField, Header("被撃状態ビヘイビア")]
    ObjectStateDamaged damagedState;

    [SerializeField, Header("破壊状態ビヘイビア")]
    ObjectStateDestroyed destroyedState;

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        if (WorldObjController.Type != WorldObjectType.StaticObject)
            return;

        staticObj = WorldObjController as ObjectController;

        DicStates = new Dictionary<ObjectStateType, WorldObjectState>();

        //要素追加
        DicStates.Add(ObjectStateType.Idle, idleState);
        DicStates.Add(ObjectStateType.Attack, attackState);
        DicStates.Add(ObjectStateType.Damaged, damagedState);
        DicStates.Add(ObjectStateType.Destroyed, destroyedState);

        //初期状態設定
        TransitionState(InitStateType);
    }

    public override bool CheckDeath()
    {
        if (!staticObj.Status.IsInvincible && staticObj.Status.CurrentHealth <= 0)
        {
            TransitionState(ObjectStateType.Destroyed);

            return true;
        }

        return false;
    }

    protected override void ResetState()
    {
        if (staticObj == null)
            return;
    }

    #region Getter & Setter

    public ObjectStateIdle IdleState
    {
        get => this.idleState;
    }

    public ObjectStateDamaged DamagedState
    {
        get => this.damagedState;
    }

    public ObjectStateDestroyed DestroyedState
    {
        get => this.destroyedState;
    }

    #endregion
}
