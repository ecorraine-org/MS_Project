using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLockOnState : CameraStateBase
{
    private CinemachineVirtualCamera _lockOnVCam;
    private CinemachineTargetGroup _targetGroup;
    private Transform _targetboss;
    private float _currentWeight;
    private const float TRANSITION_SPEED = 1f;
    private Transform _playerTransform;

    public override void OnStart()
    {

    }

    // Update is called once per frame
    public override void OnUpdate()
    {

    }

    public override void OnEnd()
    {
        //ロックオンカメラを削除
        if (_lockOnVCam != null) Object.Destroy(_lockOnVCam.gameObject);
        //ターゲットグループを削除
        if (_targetGroup != null) Object.Destroy(_targetGroup.gameObject);
    }

    //ロックオンカメラの初期化
    private void InitializeLockOnCamera()
    {
        //ロックオンカメラを生成
        var vcamObject = new GameObject("LockOn VCam");
        _lockOnVCam = vcamObject.AddComponent<CinemachineVirtualCamera>();

        //ターゲットグループを生成
        var targetGroupObject = new GameObject("TargetGroup");
        _targetGroup = targetGroupObject.AddComponent<CinemachineTargetGroup>();

        //virtualCameraの設定
        _lockOnVCam.Priority = 11; //通常のカメラよりも優先度を上げる
        _lockOnVCam.m_Lens.FieldOfView = 30f;
        _lockOnVCam.Follow = _targetGroup.transform;
        _lockOnVCam.LookAt = _targetGroup.transform;

        //Composerの設定
        var composer = _lockOnVCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (composer == null)
            composer = _lockOnVCam.AddCinemachineComponent<CinemachineFramingTransposer>();

        composer.m_DeadZoneWidth = 0.1f;
        composer.m_DeadZoneHeight = 0.1f;
        composer.m_SoftZoneWidth = 0.1f;
        composer.m_SoftZoneHeight = 0.1f;
    }

    //ボスをターゲットに設定
    //プレイヤーとボスをターゲットグループに追加
    private void SetBossTarget()
    {
        var bossObject = GameObject.FindGameObjectWithTag("Boss");
        if (bossObject != null)
        {
            _targetboss = bossObject.transform;

            //ターゲットグループにボスとプレイヤーを追加
            _targetGroup.m_Targets = new CinemachineTargetGroup.Target[]
            {
                new CinemachineTargetGroup.Target{
                    target = _context.playerTransform,
                    radius = 1f,
                    weight = 0.5f
                },
                new CinemachineTargetGroup.Target{
                    target = _targetboss,
                    radius = 1f,
                    weight = 0.5f
                }
            };
        }

    }

    private void UpdateCameraWeight()
    {
        _currentWeight = Mathf.MoveTowards(_currentWeight, 1f, TRANSITION_SPEED * Time.deltaTime);
    }

    private void UpdateTargetGroupWeights()
    {
        if (_targetGroup == null || _targetGroup.m_Targets.Length < 2) return;

        //プレイヤーとボスの距離に応じてカメラの位置を調整
        float distance = Vector3.Distance(_playerTransform.position, _targetboss.position);
        float normalizeDistance = Mathf.Clamp01(distance / 20f);

        _targetGroup.m_Targets[0].weight = Mathf.Lerp(0.7f, 0.3f, normalizeDistance);
        _targetGroup.m_Targets[1].weight = Mathf.Lerp(0.3f, 0.7f, normalizeDistance);
    }

    private void TransitionToNomalState()
    {
        //通常状態への遷移をリクエスト
        //StateManager?.Transitionto("Idle");
    }
}
