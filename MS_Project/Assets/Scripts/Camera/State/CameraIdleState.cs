using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラアイドルステート
/// </summary>
public class CameraIdleState : CameraStateBase
{
    //public CameraIdleState(CameraStateContext context) : base(context) { }

    public override void OnStart()
    {
        // カメラエフェクトの初期化
        // _context.cameraEffectController.Init(_context.cameraController);
    }

    public override void OnUpdate()
    {
        // カメラの移動
        //_context.cameraController.MoveCamera();

        // カメラの回転
        //_context.cameraController.RotateCamera();

        // カメラの衝突判定
        //_context.cameraController.CollisionCheck();

        // カメラのズーム
        //_context.cameraController.ZoomCamera();

        // カメラのエフェクト
        //_context.cameraEffectController.UpdateEffect();
    }

    public override void OnEnd()
    {
    }
}
