using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public enum CameraStateType
{
    [InspectorName("待機")] Idle,              //通常
    [InspectorName("被ダメージ")] Hit,          //被撃(被ダメージ)
    [InspectorName("死亡")] Dead,              //死亡

    [InspectorName("移動")] walk,              //移動
    [InspectorName("攻撃")] Attack,        //攻撃時
    [InspectorName("スキル発動")] Skill,        //スキル発動
    [InspectorName("終結")] FinishSkill,    //終結
    [InspectorName("捕食")] Eat,              //捕食(食べる)
    [InspectorName("モードチェンジ")] ModeChange,   //切替(モードチェンジ)
    [InspectorName("回避時")] Dodge,        //回避時
    [InspectorName("ボス討伐時")] BossSubject   //ボス討伐時
}

public class CameraStateManager : MonoBehaviour
{
    [SerializeField, Header("初期状態")]
    CameraStateType initStateType;

    [SerializeField, Header("今の状態")]
    CameraState currentState;

    [SerializeField, Header("アイドル状態ビヘイビア")]
    CameraIdleState idolState;

    [SerializeField, Header("スキル状態ビヘイビア")]
    CameraSkillState skillState;

    [SerializeField, Header("回避状態ビヘイビア")]
    CameraDodgeState dodgeState;

    [SerializeField, Header("被撃状態ビヘイビア")]
    CameraHitState hitState;

    [SerializeField, Header("死ぬ状態ビヘイビア")]
    CameraDeadState deadState;

    [SerializeField, Header("捕食状態ビヘイビア")]
    CameraEatState eatState;

    [SerializeField, Header("移動状態ビヘイビア")]
    CameraWalkState walkState;

    [SerializeField, Header("攻撃状態ビヘイビア")]
    CameraAttackState attackState;

    [SerializeField, Header("終結状態ビヘイビア")]
    CameraFinishState finishState;

    [SerializeField, Header("モードチェンジ状態ビヘイビア")]
    CameraModeChangeState modeChangeState;

    [SerializeField, Header("ボス討伐状態ビヘイビア")]
    CameraBossSubjectState bossSubjectState;

    //今のステート種類
    CameraStateType _currentStatetype;

    //前のステート種類
    CameraStateType _previousStateType;

    //辞書
    Dictionary<CameraStateType, CameraState> _stateDic = new Dictionary<CameraStateType, CameraState>();

    //CameraController
    CameraController _cameraController;

    public void Init(CameraController _cameraController)
    {
        _cameraController = _cameraController;

        _stateDic.Add(CameraStateType.Idle, idolState);
        _stateDic.Add(CameraStateType.Hit, hitState);
        _stateDic.Add(CameraStateType.Dead, deadState);
        _stateDic.Add(CameraStateType.walk, walkState);
        _stateDic.Add(CameraStateType.Attack, attackState);
        _stateDic.Add(CameraStateType.Skill, skillState);
        _stateDic.Add(CameraStateType.FinishSkill, finishState);
        _stateDic.Add(CameraStateType.Eat, eatState);
        _stateDic.Add(CameraStateType.ModeChange, modeChangeState);
        _stateDic.Add(CameraStateType.Dodge, dodgeState);
        _stateDic.Add(CameraStateType.BossSubject, bossSubjectState);

        ChangeState(initStateType);
    }

    public void ChangeState(CameraStateType _nextStateType)
    {
        if (_currentStatetype == _nextStateType) return;

        if (_stateDic.ContainsKey(_nextStateType))
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            _previousStateType = _currentStatetype;
            _currentStatetype = _nextStateType;

            currentState = _stateDic[_currentStatetype];
            currentState.Init(_cameraController);
        }
    }

    private void Update()
    {
        if (currentState == null) return;

        currentState.Tick();
    }

    private void FixedUpdate()
    {
        if (currentState == null) return;

        currentState.FixedTick();
    }

}
