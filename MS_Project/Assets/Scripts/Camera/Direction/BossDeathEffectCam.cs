using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : CameraEffectBase
{
    private Material _whiteOutMaterial;
    private int _currentCameraIndex;
    private float _whiteoutIntensity;
    private BossDeathEffectCameraData _bossDeathData;
    private CinemachineBrain _cinemachineBrain;

    protected override void OnEffectStart()
    {
        _bossDeathData = EffectData as BossDeathEffectCameraData;
        if (_bossDeathData == null)
        {
            Debug.LogError("EffectData is not BossDeathEffectCameraData");
            return;
        }
    }

    protected override void OnEffectUpdate()
    {

    }

    protected override void OnEffectStop()
    {

    }
}