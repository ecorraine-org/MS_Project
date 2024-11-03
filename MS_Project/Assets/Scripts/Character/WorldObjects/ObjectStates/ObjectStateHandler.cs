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
    PlayerIdleState idleState;

    [SerializeField, Header("スキル状態ビヘイビア")]
    PlayerSkillState skillState;

    [SerializeField, Header("回避状態ビヘイビア")]
    PlayerDodgeState dodgeState;

    [SerializeField, Header("被撃状態ビヘイビア")]
    PlayerHitState hitState;

    [SerializeField, Header("死ぬ状態ビヘイビア")]
    PlayerDeadState deadState;

    [SerializeField, Header("捕食状態ビヘイビア")]
    PlayerEatState eatState;

    [SerializeField, Header("移動状態ビヘイビア")]
    PlayerWalkState walkState;

    [SerializeField, Header("攻撃状態ビヘイビア")]
    PlayerAttackState attackState;

    [SerializeField, Header("終結状態ビヘイビア")]
    PlayerFinishState finishState;

    [SerializeField, Header("モードチェンジ状態ビヘイビア")]
    PlayerModeChangeState modeChangeState;

    //今のステート種類
    StateType currentStateType;

    //前のステート種類
    StateType preStateType;

    //辞書<キー：ステート種類、値：ステート>
    Dictionary<StateType, PlayerState> dicStates;

    //PlayerControllerの参照
    PlayerController playerController;

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;

        dicStates = new Dictionary<StateType, PlayerState>();

        //要素追加
        dicStates.Add(StateType.Idle, idleState);
        dicStates.Add(StateType.Hit, hitState);
        dicStates.Add(StateType.Eat, eatState);
        dicStates.Add(StateType.Skill, skillState);
        dicStates.Add(StateType.Dodge, dodgeState);
        dicStates.Add(StateType.Dead, deadState);
        dicStates.Add(StateType.Walk, walkState);
        dicStates.Add(StateType.Attack, attackState);
        dicStates.Add(StateType.ModeChange, modeChangeState);
        dicStates.Add(StateType.FinishSkill, finishState);


        //初期状態設定
        TransitionState(initStateType);

    }

    private void Update()
    {
        if (currentState == null) return;

        //ステート更新
        currentState.Tick();
        isAttacking = currentState.GetIsPerformDamage();
    }

    private void FixedUpdate()
    {
        if (currentState == null) return;

        //ステート更新
        currentState.FixedTick();
    }

    //状態遷移
    public void TransitionState(StateType _type)
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
        currentState.Init(playerController);
    }

    ///</summary>
    ///状態リセット処理
    ///</summary>
    private void ResetState()
    {
        playerController.AttackCollider.Reset();

        playerController.SpriteAnim.speed = 1f;

        playerController.SkillManager.Reset();

        playerController.AnimManager.Reset();
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
    ///回避状態チェック
    ///</summary>
    public bool CheckDodge()
    {
        //ボタン入力
        bool isDashTrigger = playerController.InputManager.GetDashTrigger();
        //回避中は再度回避できないようにする
        if (isDashTrigger && !playerController.SkillManager.IsDashing)
        {
            TransitionState(StateType.Dodge);
            return true;
        }

        return false;
    }

    ///<summary>
    ///攻撃状態チェック
    ///</summary>
    public bool CheckAttack()
    {    
        //ボタン入力
        bool isAttack = playerController.InputManager.GetAttackTrigger();
        if (isAttack)
        {
            TransitionState(StateType.Attack);
            return true;
        }

        return false;
    }

    ///<summary>
    ///スキル状態チェック
    ///</summary>
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

    ///<summary>
    ///捕食状態チェック
    ///</summary>
    public bool CheckEat()
    {
        //ボタン入力
        bool isEat = playerController.InputManager.GetEatTrigger();

        //フィニッシュ
        //if (isEat && playerController.DetectEnemy.CheckKillableEnemy())
        //{
        //    TransitionState(StateType.FinishSkill);
        //    return true;
        //}

        //捕食
        if (isEat
          && (playerController.SkillManager.CoolTimers[PlayerSkill.Eat] <= 0
         || playerController.StatusManager.IsFrenzy))
        {
            TransitionState(StateType.Eat);
            return true;
        }

        return false;
    }




    ///<summary>
    ///被ダメージ状態チェック
    ///</summary>
    public bool CheckHit()
    {
        if (playerController.IsHit)
        {
            playerController.IsHit = false;

            TransitionState(StateType.Hit);

            return true;
        }
        return false;
    }

    ///<summary>
    ///死亡状態チェック
    ///</summary>
    public bool CheckDeath()
    {
        if (playerController.StatusManager.Health <= 0)
        {
            TransitionState(StateType.Dead);

            return true;
        }

        return false;
    }

    #region Getter&Setter 

    public PlayerState CurrentState
    {
        get => this.currentState;
        set { this.currentState = value; }
    }

    public PlayerIdleState IdleState
    {
        get => this.idleState;

    }

    //public HitState DamagedState
    //{
    //    get => this.hitState;
    //}

    //public BlownAwayState BlownAwayState
    //{
    //    get => this.blownAwayState;
    //}

    //public DeadState DieState
    //{
    //    get => this.deadState;
    //}

    public StateType CurrentStateType
    {
        get => this.currentStateType;
    }

    public StateType PreStateType
    {
        get => this.preStateType;
    }

    #endregion
}