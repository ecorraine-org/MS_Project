using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateContext : MonoBehaviour
{
    /// <summary>
    /// カメラステート用のコンテキスト
    /// </summary>

    public Transform cameraTransform { get; set; }
    public Transform playerTransform { get; set; }
    public Transform targetTransform { get; set; }
    public CameraEffectController cameraEffectController { get; set; }
    public CameraSettings settings { get; set; }
    public Vector3 CurrentVelocity { get; set; }
    public Quaternion CurrentRotation { get; set; }

    CameraSettings Settings { get; set; }
    CameraController cameraController { get; set; }


    public CameraStateContext(Transform cameraTransform, Transform playerTransform, Transform targetTransform, CameraSettings settings)
    {
        this.cameraTransform = cameraTransform;
        this.playerTransform = playerTransform;
        this.targetTransform = targetTransform;
        this.settings = settings;
    }
}
