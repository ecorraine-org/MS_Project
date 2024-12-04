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
public abstract class WorldObjectStateHandler : MonoBehaviour
{
    [Tooltip("ObjectControllerの参照")]
    WorldObjectController worldObjController;

    [SerializeField, Header("初期状態")]
    ObjectStateType initStateType;

    [Tooltip("今のステート種類")]
    ObjectStateType currentStateType;

    [Tooltip("前のステート種類")]
    ObjectStateType preStateType;

    [SerializeField, Header("今の状態")]
    WorldObjectState currentState;

    //辞書<キー：ステート種類、値：ステート>
    Dictionary<ObjectStateType, WorldObjectState> dicStates;

    public virtual void Init(WorldObjectController _objectController)
    {
        worldObjController = _objectController;

        //初期状態設定
        //TransitionState(initStateType);
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
    public virtual void TransitionState(ObjectStateType _type)
    {
        if (DicStates[_type] == null)
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
        currentState = DicStates[_type];
        currentStateType = _type;

        //状態リセット処理
        ResetState();

        //初期化
        currentState.Init(WorldObjController);
    }

    /// <summary>
    /// 状態リセット処理
    /// </summary>
    protected abstract void ResetState();

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
        if (WorldObjController.AllowAttack)
        {
            TransitionState(ObjectStateType.Attack);

            return true;
        }

        return false;
    }

    /// <summary>
    /// 被ダメージ状態チェック
    /// </summary>
    public virtual bool CheckHit()
    {
        if (WorldObjController.IsDamaged)
        {
            WorldObjController.IsDamaged = false;

            TransitionState(ObjectStateType.Damaged);

            return true;
        }
        return false;
    }

    /// <summary>
    /// 死亡状態チェック
    /// </summary>
    public abstract bool CheckDeath();

    #region Getter & Setter

    public WorldObjectController WorldObjController
    {
        get => this.worldObjController;
    }

    protected ObjectStateType InitStateType
    {
        get => this.initStateType;
    }

    public ObjectStateType CurrentStateType
    {
        get => this.currentStateType;
    }

    public ObjectStateType PreStateType
    {
        get => this.preStateType;
    }

    public WorldObjectState CurrentState
    {
        get => this.currentState;
        set { this.currentState = value; }
    }

    public Dictionary<ObjectStateType, WorldObjectState> DicStates
    {
        get => this.dicStates;
        set { this.dicStates = value; }
    }

    #endregion
}