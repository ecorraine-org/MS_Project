using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraStateContext : MonoBehaviour
{
    /// <summary>
    /// カメラステート用のコンテキスト
    /// </summary>

    public Transform cameraTransform { get; }
    public Transform targetTransform { get; set; }
    public CameraEffectController cameraEffectController { get; }
    public CameraSettings settings { get; }
    public Vector3 CurrentVelocity { get; set; }
    public Quaternion CurrentRotation { get; set; }
    public Transform LockOnTarget { get; set; }
    public CinemachineBrain cinemachineBrain { get; }

    public CameraStateManager StateManager { get; }

    public CameraStateContext(Transform _cameraTransform,
                              Transform _targetTransform,
                            CameraSettings _settings,
                            CameraEffectController _cameraEffectController,
                            CinemachineBrain _cinemachineBrain,
                            CameraStateManager _cameraStateManager)
    {
        cameraTransform = _cameraTransform;
        targetTransform = _targetTransform;
        settings = _settings;
        cameraEffectController = _cameraEffectController;
        cinemachineBrain = _cinemachineBrain;
        StateManager = _cameraStateManager;

        CurrentVelocity = Vector3.zero;
        CurrentRotation = Quaternion.identity;
    }
}
