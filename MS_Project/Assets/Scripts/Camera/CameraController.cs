using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    //シングルトンオブジェクト
    //public static CameraController instance;

    [SerializeField, Header("カメラステートマネージャ")]
    CameraStateManager _cameraStateManager;

    [SerializeField, Header("カメラエフェクトコントローラー")]
    CameraEffectController _cameraEffectController;

    [SerializeField, Header("コリジョンハンドラ")]
    CameraCollisionHandler _cameraCollisionHandler;

    [SerializeField, Header("カメラの基本設定")]
    CameraSettings _cameraSetting;

    [Header("オフセット")]
    public Vector3 cameraOffset = new Vector3(0f, 1f, -10f);

    [Header("滑らか平均値")]
    [SerializeField, Range(0f, 1f)]
    public float blendFactor = 0.125f;
    private Transform playerPos;

    private CinemachineVirtualCamera _mainCamera;

    void Awake()
    {
        _mainCamera = GetComponent<CinemachineVirtualCamera>();
        //_mainCamera.depthTextureMode = DepthTextureMode.Depth;

        //依存性注入
        _cameraStateManager.Init(this);
        _cameraEffectController.Init(this);
        TryGetComponent(out _cameraSetting);
        transform.position = cameraOffset;
        playerPos = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = playerPos.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, blendFactor);
        transform.position = smoothedPosition;

        _mainCamera.transform.LookAt(playerPos);
    }
}
