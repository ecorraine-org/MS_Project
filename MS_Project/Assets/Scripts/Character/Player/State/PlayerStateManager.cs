using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̃X�e�[�g���
/// </summary>
public enum StateType
{
    Idle,           //�ҋ@
    Hit,            //�팂(��_���[�W)
    Dead,           //���S
    Walk,         �@//�ړ�
    Attack,         //�U��
    Skill,          //�Z�\(�X�L��)
    FinishSkill,    //���`(�K�E�Z)
    Eat,            //�\�H(�H�ׂ�)
    ModeChange,     //�ؑ�(���[�h�`�F���W)
    Dodge           //���

}

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField, Header("�������")]
    StateType initStateType;

    [SerializeField, Header("���̏��")]
    PlayerState currentState;

    [SerializeField, Header("�A�C�h����ԃr�w�C�r�A")]
    PlayerIdleState idleState;

    //[SerializeField, Header("��_���[�W��ԃr�w�C�r�A")]
    //HitState hitState;

    //[SerializeField, Header("���ʏ�ԃr�w�C�r�A")]
    //DeadState deadState;

    [SerializeField, Header("�ړ���ԃr�w�C�r�A")]
    PlayerWalkState walkState;

    [SerializeField, Header("�U����ԃr�w�C�r�A")]
    PlayerAttackState attackState;

    //���̃X�e�[�g���
    StateType currentStateType;

    //�O�̃X�e�[�g���
    StateType preStateType;

    //����<�L�[�F�X�e�[�g��ށA�l�F�X�e�[�g>
    Dictionary<StateType, PlayerState> dicStates;

    //PlayerController�̎Q��
    PlayerController playerController;

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;

        dicStates = new Dictionary<StateType, PlayerState>();

        //�v�f�ǉ�
        dicStates.Add(StateType.Idle, idleState);
        //dicStates.Add(StateType.Hit, hitState);
        //dicStates.Add(StateType.BlownAway, blownAwayState);
        //dicStates.Add(StateType.Dead, deadState);
        dicStates.Add(StateType.Walk, walkState);
        dicStates.Add(StateType.Attack, attackState);
        //dicStates.Add(StateType.CombatStance, combatStanceState);
        //dicStates.Add(StateType.Stun, stunState);

        //������Ԑݒ�
        TransitionState(initStateType);

    }

    private void Update()
    {
        if (currentState == null) return;

        //�X�e�[�g�X�V
        currentState.Tick();
    }

    private void FixedUpdate()
    {
        if (currentState == null) return;

        //�X�e�[�g�X�V
        currentState.FixedTick();
    }

    //��ԑJ��
    public void TransitionState(StateType _type)
    {
        if (dicStates[_type] == null)
        {
            print("�J�ڂ��悤�Ƃ��Ă���X�e�[�g:" + _type + "���ݒ肳��Ă��܂���");
            return;
        }

        //�I������
        if (currentState != null)
        {
            currentState.Exit();
        }

        //�O�̏�Ԃ�ۑ�����
        preStateType = currentStateType;

        //�X�e�[�g�X�V
        currentState = dicStates[_type];
        currentStateType = _type;

        //������
        currentState.Init(playerController);
    }

    ///<summary>
    ///�_���[�W�ɂ��X�e�[�g�J��
    ///</summary>
    public bool CheckDamageReaction()
    {
        //if (CheckDeath()) return true;

        if (CheckHit()) return true;

        //���̏������������Ȃ�
        return false;
    }

    ///<summary>
    ///��_���[�W��ԃ`�F�b�N
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
    ///���S��ԃ`�F�b�N
    ///</summary>
    //public bool CheckDeath()
    //{
    //    if (playerController.EnemyStatusManager.LifeBehavior.GetLife() <= 0)
    //    {
    //        TransitionState(StateType.Dead);

    //        return true;
    //    }

    //    return false;
    //}

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
