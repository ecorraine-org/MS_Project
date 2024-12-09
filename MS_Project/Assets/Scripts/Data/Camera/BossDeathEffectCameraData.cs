using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class BossDeathEffectCameraData : CameraEffectData
{
    Transform BossTransform { get; set; }
    public List<CinemachineVirtualCamera> DeathsequenceCameras { get; set; }
    public float WhiteOutDuration = 0.5f;
    public float CameraTransitionDuration = 1.0f;
    public AnimationCurve WhiteoutCurve;

    public BossDeathEffectCameraData()
    {
        EffectType = CameraEffectType.Shake;
        DeathsequenceCameras = new List<CinemachineVirtualCamera>();
        WhiteoutCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }
}
