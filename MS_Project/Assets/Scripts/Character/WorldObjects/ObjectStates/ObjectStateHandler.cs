using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public enum ObjectStateType
{
    [InspectorName("待機")] Idle,
    [InspectorName("移動")] Walk,
    [InspectorName("攻撃")] Attack,
    [InspectorName("スキル")] Skill,
    [InspectorName("被ダメージ")] Damaged,
    [InspectorName("破棄")] Destroyed,
}

public class ObjectStateHandler : MonoBehaviour
{
    [SerializeField, Header("初期状態")]
    ObjectStateType initStateType;

    [SerializeField, Header("今の状態")]
    ObjectState currentState;
    [SerializeField, ReadOnly]
    bool isAttacking;

    [SerializeField, Header("アイドル状態ビヘイビア")]
    StateIdle idleState;

    [SerializeField, Header("攻撃状態ビヘイビア")]
    StateAttack attackState;

    [SerializeField, Header("移動状態ビヘイビア")]
    StateWalk walkState;

    [SerializeField, Header("被撃状態ビヘイビア")]
    StateDamaged damagedState;

    [SerializeField, Header("死ぬ状態ビヘイビア")]
    StateDestroyed destroyedState;

    //今のステート種類
    ObjectStateType currentStateType;

    //前のステート種類
    ObjectStateType preStateType;

    //辞書<キー：ステート種類、値：ステート>
    Dictionary<ObjectStateType, ObjectState> dicStates;

    //PlayerControllerの参照
    ObjectController objController;

    public void Init(ObjectController _objectController)
    {
        objController = _objectController;

        dicStates = new Dictionary<ObjectStateType, ObjectState>();

        //要素追加
        dicStates.Add(ObjectStateType.Idle, idleState);
        dicStates.Add(ObjectStateType.Walk, walkState);
        dicStates.Add(ObjectStateType.Attack, attackState);
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

    //状態遷移
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

    ///</summary>
    ///状態リセット処理
    ///</summary>
    private void ResetState()
    {
        /*
        playerController.AttackCollider.Reset();

        playerController.SpriteAnim.speed = 1f;

        playerController.SkillManager.Reset();

        playerController.AnimManager.Reset();
        */
    }

    ///<summary>
    ///ダメージによるステート遷移
    ///</summary>
    public bool CheckDamageReaction()
    {
        //if (CheckDeath()) return true;

        if (CheckHit()) return true;

        //何の条件も満たさない
        return false;
    }

    ///<summary>
    ///攻撃状態チェック
    ///</summary>
    public bool CheckAttack()
    {    
        //ボタン入力
        bool isAttack = objController.CanAttack;
        if (isAttack)
        {
            TransitionState(ObjectStateType.Attack);
            return true;
        }

        return false;
    }

    ///<summary>
    ///スキル状態チェック
    ///</summary>
    /*
    public bool CheckSkill()
    {
        //ボタン入力
        bool isSkill = playerController.InputManager.GetSkillTrigger();
        PlayerSkill skill = (PlayerSkill)(int)playerController.ModeManager.Mode;
        if (isSkill && playerController.SkillManager.CoolTimers[skill] <= 0
           && playerController.SkillManager.HpCost(skill))

        {
            TransitionState(StateType.Skill);
            return true;
        }

        return false;
    }
     */

    ///<summary>
    ///被ダメージ状態チェック
    ///</summary>
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

    ///<summary>
    ///死亡状態チェック
    ///</summary>
    public bool CheckDeath()
    {
        if (objController.status.Health <= 0)
        {
            TransitionState(ObjectStateType.Destroyed);

            return true;
        }

        return false;
    }

    #region Getter&Setter 

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