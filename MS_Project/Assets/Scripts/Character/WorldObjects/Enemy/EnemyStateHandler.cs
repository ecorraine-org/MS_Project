using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateHandler : WorldObjectStateHandler
{
    EnemyController enemy;

    [SerializeField, Header("アイドル状態ビヘイビア")]
    EnemyStateIdle idleState;

    [SerializeField, Header("移動状態ビヘイビア")]
    EnemyStateWalk walkState;

    [SerializeField, Header("攻撃状態ビヘイビア")]
    EnemyStateAttack attackState;

    [SerializeField, Header("スキル使用状態ビヘイビア")]
    EnemyStateSkill skillState;

    [SerializeField, Header("被撃状態ビヘイビア")]
    EnemyStateDamaged damagedState;

    [SerializeField, Header("破壊状態ビヘイビア")]
    EnemyStateDestroyed destroyedState;

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        if (WorldObjController.Type != WorldObjectType.Enemy)
            return;

        enemy = WorldObjController as EnemyController;
                //staticObj = worldObjController as ObjectController;

        DicStates = new Dictionary<ObjectStateType, WorldObjectState>();

        //要素追加
        DicStates.Add(ObjectStateType.Idle, idleState);
        DicStates.Add(ObjectStateType.Walk, walkState);
        DicStates.Add(ObjectStateType.Attack, attackState);
        DicStates.Add(ObjectStateType.Skill, skillState);
        DicStates.Add(ObjectStateType.Damaged, damagedState);
        DicStates.Add(ObjectStateType.Destroyed, destroyedState);

        //初期状態設定
        TransitionState(InitStateType);
    }

    /// <summary>
    /// スキル状態チェック
    /// </summary>
    public bool CheckSkill()
    {
        bool isSkill = enemy.UsingSkill;
        if (isSkill)
        {
            TransitionState(ObjectStateType.Skill);

            return true;
        }

        return false;
    }

    public override bool CheckDeath()
    {
        if (enemy.Status.CurrentHealth <= 0)
        {
            TransitionState(ObjectStateType.Destroyed);

            return true;
        }

        return false;
    }

    protected override void ResetState()
    {
        if (enemy == null)
            return;

        enemy.AttackCollider.Reset();

        enemy.AnimManager.Reset();

        enemy.SkillManager.Reset();
    }

    #region Getter & Setter

    public EnemyStateIdle IdleState
    {
        get => this.idleState;
    }

    public EnemyStateDamaged DamagedState
    {
        get => this.damagedState;
    }

    public EnemyStateDestroyed DestroyedState
    {
        get => this.destroyedState;
    }

    #endregion
}
