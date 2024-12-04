using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateManager
{
    private ICameraState _currentState;
    private readonly Dictionary<string, ICameraState> _states;
    private CameraStateContext _context;

    public ICameraState CurrentState => _currentState;

    public CameraStateManager()
    {
        _states = new Dictionary<string, ICameraState>();
    }

    public void Initialize(CameraStateContext context)
    {
        _context = context;
        InitializeStates();
    }

    /// <summary>
    /// ステートを登録
    /// </summary>
    private void InitializeStates()
    {
        RegisterState("Idle", new CameraIdleState());
        //RegisterState("Walk", new CameraWalkState());
        RegisterState("LockOn", new CameraLockOnState());
    }

    public void RegisterState(string stateName, ICameraState state)
    {
        if (_states.ContainsKey(stateName))
        {
            Debug.LogError("State is already registered.");
            return;
        }
        _states[stateName] = state;

        if (state is CameraStateBase baseState)
        {
            //baseState.SetStateManager(this);
        }
    }

    public void TransitionTo(string stateName)
    {
        // ステートが登録されていない場合はエラーを出力
        if (!_states.ContainsKey(stateName))
        {
            Debug.LogError("State is not registered.");
            return;
        }

        // 現在のステートがある場合はExitStateを呼び出す
        if (_currentState != null)
        {
            _currentState.ExitState(_context);
        }
        Debug.Log($"Transition to {stateName}");

        // 次のステートに遷移
        _currentState?.ExitState(_context);
        _currentState = _states[stateName];
        _currentState.EnterState(_context);
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public void Update()
    {
        _currentState?.UpdateState(_context);
    }

    // ステートのコンテキストを取得
    public CameraStateContext GetContext() => _context;
}
