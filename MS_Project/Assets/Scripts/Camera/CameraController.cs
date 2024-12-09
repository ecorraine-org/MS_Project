using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    //シングルトンオブジェクト
    //public static CameraController instance;

    private CinemachineBrain _cinemachineBrain;

    [SerializeField, Header("カメラステートマネージャ")]
    CameraStateManager _cameraStateManager;

    [SerializeField, Header("カメラエフェクトコントローラー")]
    CameraEffectController _cameraEffectController;

    [SerializeField, Header("カメラエフェクトマネージャー")]
    CameraEffectManager _cameraEffectManager;

    [SerializeField, Header("コリジョンハンドラ")]
    CameraCollisionHandler _cameraCollisionHandler;

    [SerializeField, Header("カメラの基本設定")]
    CameraSettings _cameraSetting;

    [Header("オフセット")]
    public Vector3 cameraOffset = new Vector3(0f, 1f, -10f);

    [Header("滑らか平均値")]
    [SerializeField, Range(0f, 1f)]

    [Header("PlayerStateObserver")]
    private IPlayerStateObserver _playerStateObserver;

    private Camera _camera;

    [SerializeField, Header("ターゲット")]
    private Transform _target;
    private Transform _lockOnTarget;
    private bool _isInitialized;

    public float blendFactor = 0.125f;
    private Transform playerPos;

    private CinemachineVirtualCamera _mainCamera;

    void Awake()
    {
        _mainCamera = GetComponent<CinemachineVirtualCamera>();
        //_mainCamera.depthTextureMode = DepthTextureMode.Depth;

        //依存性注入
        // _cameraStateManager.Init(this);
        //_cameraEffectController.Init(this);
        //TryGetComponent(out _cameraSetting);
        transform.position = cameraOffset;
        playerPos = GameObject.FindWithTag("Player").transform;
        _target = playerPos;

        //CinemachineBrainを取得
        _cinemachineBrain = gameObject.AddComponent<CinemachineBrain>();
        // _cinemachineBrain.m_UpdateMethod = _cinemachineBrain.UpdateMethod.LateUpdate;

        //LockOnState
        Initialize();

    }

    void FixedUpdate()
    {

    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Initialize()
    {
        if (_isInitialized) return;

        InitializeComponents();
        InitializeStateManager();


        _isInitialized = true;
    }

    /// <summary>
    /// コンポーネントの初期化
    /// Initializeメソッドで呼び出す
    /// </summary>
    private void InitializeComponents()
    {
        _camera = GetComponent<Camera>();
        _cameraEffectController = new CameraEffectController();
        _cameraEffectManager = new CameraEffectManager();
        Debug.Log("CameraEffectController: " + _cameraEffectController);
        //_cameraCollisionHandler = new CameraCollisionHandler(_settings, _cameraTransform, _targetTransform);
        _cameraStateManager = new CameraStateManager();

        ConfigureCamera();

    }

    /// <summary>
    /// ステートマネージャの初期化
    /// Initializeメソッドで呼び出す
    /// </summary>
    private void InitializeStateManager()
    {
        var context = new CameraStateContext(
            transform,
            _target,
            _cameraSetting,
            _cameraEffectController,
            _cinemachineBrain,
            _cameraStateManager
        );

        _cameraStateManager.Initialize(context);
        _cameraStateManager.TransitionTo("Idle");
        _cameraEffectController.Initialize(context);

    }

    /// <summary>
    /// カメラの設定を行う
    /// InitializeComponentsメソッドで呼び出す
    /// </summary>
    private void ConfigureCamera()
    {
        var basicSettings = _cameraSetting.BasicSettings;
        _camera.fieldOfView = basicSettings.FieldOfView;
        _camera.nearClipPlane = basicSettings.NearClipPlane;
        _camera.farClipPlane = basicSettings.FarClipPlane;
        _camera.cullingMask = basicSettings.CullingMask;
    }

    private void Update()
    {
        _cameraStateManager?.Update();
        _cameraEffectController?.Update();

    }

    /// <summary>
    /// カメラのコリジョンを処理する
    /// Updateメソッドで呼び出す
    /// </summary>
    private void HandleCollision()
    {

    }

    /// <summary>
    /// カメラのエフェクトを適用する
    /// Updateメソッドで呼び出す
    /// </summary>
    private void ApplyEffects()
    {

    }

    public void SetTarget(Transform target)
    {
        _target = target;
        var context = _cameraStateManager.GetContext();
        if (context != null)
        {
            context.targetTransform = target;
        }
    }

    public void ToggleLockOn()
    {

    }

    private bool TryFindLockOnTarget(out Transform target)
    {
        target = null;
        return false;
    }
}
