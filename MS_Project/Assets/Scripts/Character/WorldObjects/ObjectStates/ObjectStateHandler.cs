using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// オブジェクトの状態
/// </summary>
public enum ObjectStateType
{
    [InspectorName("待機"), Tooltip("待機")] Idle,
    [InspectorName("移動"), Tooltip("移動")] Walk,
    [InspectorName("攻撃"), Tooltip("攻撃")] Attack,
    [InspectorName("スキル"), Tooltip("スキル")] Skill,
    [InspectorName("被ダメージ"), Tooltip("被ダメージ")] Damaged,
    [InspectorName("破棄"), Tooltip("破棄")] Destroyed,
}

/// <summary>
/// オブジェクトの状態管理
/// </summary>
public class ObjectStateHandler : MonoBehaviour
{
    [SerializeField, Header("初期状態")]
    ObjectStateType initStateType;

    [SerializeField, Header("今の状態")]
    ObjectState currentState;

    [SerializeField, Header("アイドル状態ビヘイビア")]
    StateIdle idleState;

    [SerializeField, Header("移動状態ビヘイビア")]
    StateWalk walkState;

    [SerializeField, Header("攻撃状態ビヘイビア")]
    StateAttack attackState;

    [SerializeField, Header("スキル使用状態ビヘイビア")]
    StateSkill skillState;

    [SerializeField, Header("被撃状態ビヘイビア")]
    StateDamaged damagedState;

    [SerializeField, Header("破壊状態ビヘイビア")]
    StateDestroyed destroyedState;

    [Tooltip("今のステート種類")]
    ObjectStateType currentStateType;

    [Tooltip("前のステート種類")]
    ObjectStateType preStateType;

    //辞書<キー：ステート種類、値：ステート>
    Dictionary<ObjectStateType, ObjectState> dicStates;

    [Tooltip("ObjectControllerの参照")]
    WorldObjectController objController;

    EnemyController enemy;

    public void Init(WorldObjectController _objectController)
    {
        objController = _objectController;

        if (objController.Type == WorldObjectType.Enemy)
            enemy = objController as EnemyController;

        dicStates = new Dictionary<ObjectStateType, ObjectState>();

        //要素追加
        dicStates.Add(ObjectStateType.Idle, idleState);
        dicStates.Add(ObjectStateType.Walk, walkState);
        dicStates.Add(ObjectStateType.Attack, attackState);
        dicStates.Add(ObjectStateType.Skill, skillState);
        dicStates.Add(ObjectStateType.Damaged, damagedState);
        dicStates.Add(ObjectStateType.Destroyed, destroyedState);

        //初期状態設定
        TransitionState(initStateType);
    }

    private void Update()
    {
        if (currentState == null) return;

        //ステート更新
        currentState.Tick();
    }

    private void FixedUpdate()
    {
        if (currentState == null) return;

        //ステート更新
        currentState.FixedTick();
    }

    /// <summary>
    /// 状態遷移
    /// </summary>
    public void TransitionState(ObjectStateType _type)
    {
        if (dicStates[_type] == null)
        {
            print("遷移しようとしているステート:" + _type + "が設定されていません");
            return;
        }

        //終了処理
        if (currentState != null)
        {
            currentState.Exit();
        }

        //前の状態を保存する
        preStateType = currentStateType;

        //ステート更新
        currentState = dicStates[_type];
        currentStateType = _type;

        //状態リセット処理
        ResetState();

        //初期化
        currentState.Init(objController);
    }

    /// <summary>
    /// 状態リセット処理
    /// </summary>
    private void ResetState()
    {
        objController.AttackCollider.Reset();

        enemy.AnimManager.Reset();

        enemy.SkillManager.Reset();
    }

    /// <summary>
    /// ダメージによるステート遷移
    /// </summary>
    public bool CheckDamageReaction()
    {
        if (CheckHit()) return true;

        //何の条件も満たさない
        return false;
    }

    /// <summary>
    /// 攻撃状態チェック
    /// </summary>
    public bool CheckAttack()
    {
        if (objController.AllowAttack)
        {
            TransitionState(ObjectStateType.Attack);

            return true;
        }

        return false;
    }

    /// <summary>
    /// スキル状態チェック
    /// </summary>
    public bool CheckSkill()
    {
        bool isSkill = objController.UsingSkill;
        if (isSkill)
        {
            TransitionState(ObjectStateType.Skill);

            return true;
        }

        return false;
    }

    /// <summary>
    /// 被ダメージ状態チェック
    /// </summary>
    public bool CheckHit()
    {
        if (objController.IsDamaged)
        {
            objController.IsDamaged = false;

            TransitionState(ObjectStateType.Damaged);

            return true;
        }
        return false;
    }

    /// <summary>
    /// 死亡状態チェック
    /// </summary>
    public bool CheckDeath()
    {
        if (objController.Type == WorldObjectType.Enemy)
        {
            EnemyController enemy = objController as EnemyController;
            if (enemy.EnemyStatus.CurrentHealth <= 0)
            {
                TransitionState(ObjectStateType.Destroyed);

                return true;
            }
        }

        else if (objController.Type == WorldObjectType.StaticObject)
        {
            ObjectController objectController = objController as ObjectController;
            if (!objectController.ObjectStatus.IsInvincible)
            {
                TransitionState(ObjectStateType.Destroyed);

                return true;
            }
        }

        return false;
    }

    #region Getter & Setter

    public ObjectState CurrentState
    {
        get => this.currentState;
        set { this.currentState = value; }
    }

    public StateIdle IdleState
    {
        get => this.idleState;
    }

    public StateDamaged DamagedState
    {
        get => this.damagedState;
    }

    public StateDestroyed DestroyedState
    {
        get => this.DestroyedState;
    }

    public ObjectStateType CurrentStateType
    {
        get => this.currentStateType;
    }

    public ObjectStateType PreStateType
    {
        get => this.preStateType;
    }

    #endregion
}