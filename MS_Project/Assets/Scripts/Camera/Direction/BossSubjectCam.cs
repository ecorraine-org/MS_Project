using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// ボス死亡時のカメラエフェクト
/// </summary>
/// 
public class BossSubjectCam : CameraEffectBase
{
    private float _currentPhaseTime = 0f;  // 現在のフェーズ時間
    private const int TOTAL_PHASE = 4;  // フェーズ数 ホワイトアウト+3カメ
    private List<CinemachineVirtualCamera> _DeathCameras;
    private Material _whiteOutMaterial;
    private float _whiteOutTime = 0.5f;  // ホワイトアウト時間 

    /// <summary>
    /// エフェクト再生
    /// </summary>
    /// <param name="_data"></param>
    public override void Play(CameraEffectData _data)
    {

    }


    //死亡時に一瞬ホワイトアウトし、
    //その後3カメラで、順番にカメラを切り替える
    protected override void OnEffectStart()
    {
        //エフェクト開始時の処理

    }

    protected override void OnEffectUpdate()
    {
        //エフェクト更新時の処理
    }

    protected override void OnEffectStop()
    {
        //エフェクト終了時の処理
    }

    private void WhiteOut()
    {
        //ホワイトアウト処理
        //画面全体を白のフェードで覆う


    }
}
