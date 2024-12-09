using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CameraIdleState.cs
public class CameraIdleState : CameraStateBase
{
    private Vector3 _positionVelocity;
    private float _currentRotationVelocity;
    private Vector3 _currentRotationAngles;
    private Vector3 _targetRotationAngles;

    protected override void OnStateEnter()
    {
        _positionVelocity = Vector3.zero;
        _currentRotationVelocity = 0f;
        _currentRotationAngles = CameraTransform.eulerAngles;
        _targetRotationAngles = _currentRotationAngles;
    }

    protected override void OnStateUpdate()
    {
        if (TargetTransform == null) return;

        var followSettings = Settings.FollowSettings;
        var targetPosition = CalculateTargetPosition(followSettings);
        var targetRotation = CalculateTargetRotation(followSettings);

        // 位置の更新
        var smoothedPosition = Vector3.SmoothDamp(
            CameraTransform.position,
            targetPosition,
            ref _positionVelocity,
            followSettings.PositionSmoothTime,
            float.MaxValue,
            Time.smoothDeltaTime);  // Time.smoothDeltaTimeを使用

        // 回転の更新
        _targetRotationAngles = targetRotation.eulerAngles;
        _currentRotationAngles = new Vector3(
            Mathf.SmoothDampAngle(_currentRotationAngles.x, _targetRotationAngles.x, ref _currentRotationVelocity, followSettings.RotationSmoothTime),
            Mathf.SmoothDampAngle(_currentRotationAngles.y, _targetRotationAngles.y, ref _currentRotationVelocity, followSettings.RotationSmoothTime),
            Mathf.SmoothDampAngle(_currentRotationAngles.z, _targetRotationAngles.z, ref _currentRotationVelocity, followSettings.RotationSmoothTime)
        );

        // 最終的な位置と回転の適用
        CameraTransform.position = smoothedPosition;
        CameraTransform.rotation = Quaternion.Euler(_currentRotationAngles);
    }

    private Vector3 CalculateTargetPosition(CameraFollowSettings settings)
    {
        var targetPosition = TargetTransform.position + settings.PositionOffset;

        // 位置の制約適用
        if (settings.LockX) targetPosition.x = CameraTransform.position.x;
        if (settings.LockY) targetPosition.y = CameraTransform.position.y;
        if (settings.LockZ) targetPosition.z = CameraTransform.position.z;

        return targetPosition;
    }

    private Quaternion CalculateTargetRotation(CameraFollowSettings settings)
    {
        var lookAtPosition = TargetTransform.position + settings.LookAtOffset;
        var direction = (lookAtPosition - CameraTransform.position).normalized;
        return Quaternion.LookRotation(direction);
    }
}
