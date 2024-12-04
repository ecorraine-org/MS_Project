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
        //EffectController.StopAllEffects();
    }

    protected override void OnStateUpdate()
    {
        if (TargetTransform == null) return;

        var followSettings = Settings.FollowSettings;
        var targetPosition = TargetTransform.position + followSettings.PositionOffset;
        var targetRotation = Quaternion.LookRotation(
            (TargetTransform.position + followSettings.LookAtOffset - CameraTransform.position).normalized);

        ApplyPositionConstraints(ref targetPosition, followSettings);
        ApplySmoothMovement(targetPosition, targetRotation, followSettings);
    }

    private void ApplyPositionConstraints(ref Vector3 targetPosition, CameraFollowSettings settings)
    {
        if (settings.LockX) targetPosition.x = CameraTransform.position.x;
        if (settings.LockY) targetPosition.y = CameraTransform.position.y;
        if (settings.LockZ) targetPosition.z = CameraTransform.position.z;
    }

    private void ApplySmoothMovement(Vector3 targetPosition, Quaternion targetRotation, CameraFollowSettings settings)
    {
        CameraTransform.position = Vector3.SmoothDamp(
            CameraTransform.position,
            targetPosition,
            ref _positionVelocity,
            settings.PositionSmoothTime);

        CameraTransform.rotation = Quaternion.Slerp(
            CameraTransform.rotation,
            targetRotation,
            Time.deltaTime / settings.RotationSmoothTime);
    }
}
