using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLockOnState : CameraStateBase
{
    private Vector3 _positionVelocity;
    private Vector3 _rotationVelocity;
    private Transform _lockOnTarget;
    private CinemachineVirtualCamera _lockOnCamera;
    private CinemachineTargetGroup _targetGroup;

    protected override void OnStateEnter()
    {
        _positionVelocity = Vector3.zero;
        _rotationVelocity = Vector3.zero;
        _lockOnTarget = StateManager.GetContext().LockOnTarget;

        InitializeLockOnCamera();
        PlayLockOnEffect();
    }

    private void InitializeLockOnCamera()
    {
        if (_lockOnCamera != null) return;

        var lockOnSettings = Settings.LockOnSettings;

        // Virtual Cameraのセットアップ
        var vcamObject = new GameObject("LockOn VCam");
        _lockOnCamera = vcamObject.AddComponent<CinemachineVirtualCamera>();

        // Target Groupのセットアップ
        var groupObject = new GameObject("Target Group");
        _targetGroup = groupObject.AddComponent<CinemachineTargetGroup>();

        _targetGroup.m_Targets = new CinemachineTargetGroup.Target[]
        {
            new CinemachineTargetGroup.Target { target = TargetTransform, weight = lockOnSettings.PlayerWeight, radius = 2f },
            new CinemachineTargetGroup.Target { target = _lockOnTarget, weight = lockOnSettings.TargetWeight, radius = 3f }
        };

        _lockOnCamera.Follow = _targetGroup.transform;
        _lockOnCamera.LookAt = _targetGroup.transform;
        ConfigureVirtualCamera(_lockOnCamera);
    }

    private void ConfigureVirtualCamera(CinemachineVirtualCamera vcam)
    {
        var composer = vcam.GetCinemachineComponent<CinemachineComposer>();
        if (composer == null)
            composer = vcam.AddCinemachineComponent<CinemachineComposer>();

        composer.m_DeadZoneWidth = 0.1f;
        composer.m_DeadZoneHeight = 0.1f;
        composer.m_SoftZoneWidth = 0.5f;
        composer.m_SoftZoneHeight = 0.5f;
    }

    private void PlayLockOnEffect()
    {
        var transitionEffect = new CameraEffectData
        {
            Duration = 0.3f,
            Intensity = 0.3f
        };
        //EffectController.PlayEffect("LockOnTransition", transitionEffect);
    }

    protected override void OnStateUpdate()
    {
        if (_lockOnTarget == null || TargetTransform == null)
        {
            StateManager.TransitionTo("Idle");
            return;
        }

        UpdateTargetGroupWeights();
        UpdateCameraPosition();
    }

    private void UpdateTargetGroupWeights()
    {
        if (_targetGroup == null || _targetGroup.m_Targets.Length < 2) return;

        var distance = Vector3.Distance(TargetTransform.position, _lockOnTarget.position);
        var normalizedDistance = Mathf.Clamp01(distance / Settings.LockOnSettings.MaxLockOnDistance);

        _targetGroup.m_Targets[0].weight = Mathf.Lerp(0.7f, 0.3f, normalizedDistance);
        _targetGroup.m_Targets[1].weight = Mathf.Lerp(0.3f, 0.7f, normalizedDistance);
    }

    private void UpdateCameraPosition()
    {
        var midPoint = Vector3.Lerp(TargetTransform.position, _lockOnTarget.position, 0.5f);
        var targetPosition = midPoint + Settings.LockOnSettings.OffsetFromTarget;

        CameraTransform.position = Vector3.SmoothDamp(
            CameraTransform.position,
            targetPosition,
            ref _positionVelocity,
            Settings.FollowSettings.PositionSmoothTime);

        var lookDirection = (_lockOnTarget.position - CameraTransform.position).normalized;
        var targetRotation = Quaternion.LookRotation(lookDirection);
        CameraTransform.rotation = Quaternion.Slerp(
            CameraTransform.rotation,
            targetRotation,
            Time.deltaTime / Settings.FollowSettings.RotationSmoothTime);
    }

    protected override void OnStateExit()
    {
        if (_lockOnCamera != null)
            Object.Destroy(_lockOnCamera.gameObject);

        if (_targetGroup != null)
            Object.Destroy(_targetGroup.gameObject);
    }
}