using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectManager : MonoBehaviour
{
    public static CameraEffectManager Instance { get; private set; }

    [SerializeField] private Camera _targetCamera;
    private Queue<ICameraEffectCommand> _effectQueue = new Queue<ICameraEffectCommand>();
    private ICameraEffectCommand _currentEffect;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (_targetCamera == null)
        {
            _targetCamera = Camera.main;
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (_currentEffect == null && _effectQueue.Count > 0)
        {
            _currentEffect = _effectQueue.Dequeue();
            Debug.Log($"Starting camera effect: {_currentEffect.GetType().Name}");
        }

        if (_currentEffect != null)
        {
            _currentEffect.Execute(_targetCamera);
            if (_currentEffect.IsComplete())
            {
                Debug.Log($"Completed camera effect: {_currentEffect.GetType().Name}");
                _currentEffect = null;
            }
        }
    }

    public void AddEffect(ICameraEffectCommand effect)
    {
        if (effect == null)
        {
            Debug.LogError("Attempted to add null effect!");
            return;
        }

        _effectQueue.Enqueue(effect);
        Debug.Log($"Added camera effect to queue: {effect.GetType().Name}");
    }
}