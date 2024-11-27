using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// ミニマップ作成用カメラ。
/// Playerに追従しながらミニマップテクスチャを描画します。
/// </summary>
[ExecuteInEditMode]
public class MinimapCamera : MonoBehaviour
{
    /// <summary>
    /// 自分のカメラを取得する
    /// </summary>
    public Camera myCamera
    {
        get
        {
            if (!_myCamera)
            {
                _myCamera = GetComponent<Camera>();
            }
            return _myCamera;
        }
    }

    [SerializeField, Tooltip("追従対象のPlayerオブジェクト")]
    private Transform _player;

    [SerializeField, Tooltip("カメラとPlayerのオフセット")]
    private Vector3 _offset = new Vector3(0, 10, 0);

    [SerializeField, Tooltip("ミニマップに適用するマテリアル")]
    private Material _minimapMaterial;

    private Camera _myCamera;

    private void Awake()
    {
        myCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    private void OnEnable()
    {
        UpdateMinimapTexture();
    }

    private void Update()
    {
        FollowPlayer();
    }

    /// <summary>
    /// ミニマップテクスチャを更新する
    /// </summary>
    public void UpdateMinimapTexture()
    {
        // エディター上でEditModeの時は無効化しない
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying) { return; }
#endif
        if (gameObject.activeInHierarchy)
        {
            myCamera.enabled = true;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _minimapMaterial);
    }

    /// <summary>
    /// Playerに追従する
    /// </summary>
    private void FollowPlayer()
    {
        if (_player != null)
        {
            transform.position = _player.position + _offset;
        }
    }
}
