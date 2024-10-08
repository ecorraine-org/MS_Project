using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// プレイヤーのステート種類
/// </summary>
public enum StateType
{
    [InspectorName("待機")] Idle,                 //待機
    [InspectorName("被ダメージ")] Hit,             //被撃(被ダメージ)
    [InspectorName("死亡")] Dead,                 //死亡
    [InspectorName("移動")] Walk,                 //移動
    [InspectorName("攻撃")] Attack,               //攻撃
    [InspectorName("スキル")] Skill,               //技能(スキル)
    [InspectorName("必殺技")] FinishSkill,         //奥義(必殺技)
    [InspectorName("捕食")] Eat,                  //捕食(食べる)
    [InspectorName("モードチェンジ")] ModeChange,   //切替(モードチェンジ)
    [InspectorName("回避")] Dodge                 //回避

}

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField, Header("初期状態")]
    StateType initStateType;

    [SerializeField, Header("今の状態")]
    PlayerState currentState;
    [SerializeField, ReadOnly]
    bool isAttacking;

    [SerializeField, Header("アイドル状態ビヘイビア")]
    PlayerIdleState idleState;

    [SerializeField, Header("スキル状態ビヘイビア")]
    PlayerSkillState skillState;

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
        dicStates.Add(StateType.Dead, deadState);
        dicStates.Add(StateType.Walk, walkState);
        dicStates.Add(StateType.Attack, attackState);
        dicStates.Add(StateType.ModeChange, modeChangeState);
        //dicStates.Add(StateType.CombatStance, combatStanceState);
        //dicStates.Add(StateType.Stun, stunState);

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
        playerController.AttackCollider.CanHit = false;
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
