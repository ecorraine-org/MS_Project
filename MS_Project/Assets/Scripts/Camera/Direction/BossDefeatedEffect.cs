using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossDefeatedEffect : MonoBehaviour
{
    private readonly float _duration;
    private float _elapsedTime;
    private CinemachineVirtualCamera _dramaticCamera;
    private Transform _bossTransform;
    private bool _isInitialized;
    private WhiteoutEffect _whiteoutEffect;

    public BossDefeatedEffect(Transform bossTransform, float duration = 3f)
    {
        _bossTransform = bossTransform;
        _duration = duration;
        _whiteoutEffect = new WhiteoutEffect(0.5f); // 0.5秒のホワイトアウト
    }

    public void Execute(Camera camera)
    {
        if (!_isInitialized)
        {
            InitializeDramaticCamera();
            _isInitialized = true;
        }

        UpdateEffect();
    }

    private void InitializeDramaticCamera()
    {
        var go = new GameObject("BossDefeatCamera");
        _dramaticCamera = go.AddComponent<CinemachineVirtualCamera>();
        _dramaticCamera.Priority = 100; // 最高優先度

        var composer = _dramaticCamera.GetCinemachineComponent<CinemachineComposer>();
        composer.m_TrackedObjectOffset = new Vector3(0, 2f, 0);

        _dramaticCamera.Follow = _bossTransform;
        _dramaticCamera.LookAt = _bossTransform;

        // ドラマチックな演出のためのカメラ設定
        var transposer = _dramaticCamera.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset = new Vector3(5f, 2f, -3f);
    }

    private void UpdateEffect()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime < 0.5f) // 最初の0.5秒はホワイトアウト
        {
            _whiteoutEffect.Execute(Camera.main);
        }
    }

    public bool IsComplete()
    {
        return _elapsedTime >= _duration;
    }

    public void Terminate()
    {
        if (_dramaticCamera != null)
        {
            Object.Destroy(_dramaticCamera.gameObject);
        }
    }
}
