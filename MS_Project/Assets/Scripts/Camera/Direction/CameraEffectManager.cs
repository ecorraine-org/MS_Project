using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectManager : MonoBehaviour
{
    private static CameraEffectManager _instance;
    public static CameraEffectManager Instance => _instance;

    private Dictionary<string, ICameraEffectCommand> _activeEffects = new();
    private List<ICameraEffectObserver> _observers = new();
    private Camera _mainCamera;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        _mainCamera = Camera.main;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        var completedEffects = new List<string>();

        foreach (var effect in _activeEffects)
        {
            effect.Value.Execute(_mainCamera);

            if (effect.Value.IsComplete())
            {
                completedEffects.Add(effect.Key);
            }
        }

        foreach (var effectName in completedEffects)
        {
            _activeEffects[effectName].Terminate();
            _activeEffects.Remove(effectName);
            NotifyEffectCompleted(effectName);
        }
    }

    /// <summary>
    /// エフェクトを実行する
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="effect"></param>
    public void ExecuteEffect(string effectName, ICameraEffectCommand effect)
    {
        if (_activeEffects.ContainsKey(effectName))
        {
            _activeEffects[effectName].Terminate();
            _activeEffects.Remove(effectName);
        }

        _activeEffects.Add(effectName, effect);
        NotifyEffectStarted(effectName);
    }


    /// <summary>
    /// オブザーバーを登録する
    /// </summary>
    public void RegisterObserver(ICameraEffectObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    /// <summary>
    /// オブザーバーを登録解除する
    /// </summary>

    public void UnregisterObserver(ICameraEffectObserver observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
        }
    }

    /// <summary>
    /// エフェクト開始通知
    /// </summary>
    private void NotifyEffectStarted(string effectName)
    {
        foreach (var observer in _observers)
        {
            observer.OnEffectStarted(effectName);
        }
    }

    /// <summary>
    /// エフェクト完了通知
    /// </summary>
    private void NotifyEffectCompleted(string effectName)
    {
        foreach (var observer in _observers)
        {
            observer.OnEffectCompleted(effectName);
        }
    }
}
