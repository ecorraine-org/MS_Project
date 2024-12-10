using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CameraIdleState.cs
public class CameraIdleState : CameraStateBase
{
    private Vector3 _positionVelocity;
    private Vector3 _rotationVelocity;

    protected override void OnStateEnter()
    {
        _positionVelocity = Vector3.zero;
        _rotationVelocity = Vector3.zero;
    }

    protected override void OnStateUpdate()
    {
        if (TargetTransform == null) return;

        var followSettings = Settings.FollowSettings;
        var targetPosition = CalculateTargetPosition(followSettings);
        var targetRotation = CalculateTargetRotation(followSettings);

        // 位置の更新
        CameraTransform.position = Vector3.SmoothDamp(
            CameraTransform.position,
            targetPosition,
            ref _positionVelocity,
            followSettings.PositionSmoothTime);

        // 回転の更新
        CameraTransform.rotation = Quaternion.Slerp(
            CameraTransform.rotation,
            targetRotation,
            Time.deltaTime / followSettings.RotationSmoothTime);
    }

    private Vector3 CalculateTargetPosition(CameraFollowSettings settings)
    {
        var targetPosition = TargetTransform.position + settings.PositionOffset;

        if (settings.LockX) targetPosition.x = CameraTransform.position.x;
        if (settings.LockY) targetPosition.y = CameraTransform.position.y;
        if (settings.LockZ) targetPosition.z = CameraTransform.position.z;

        return targetPosition;
    }

    private Quaternion CalculateTargetRotation(CameraFollowSettings settings)
    {
        var lookAtPosition = TargetTransform.position + settings.LookAtOffset;
        var direction = lookAtPosition - CameraTransform.position;

        // 方向ベクトルが0の場合の対処
        if (direction.magnitude < 0.001f)
        {
            return CameraTransform.rotation;
        }

        return Quaternion.LookRotation(direction.normalized);
    }
}