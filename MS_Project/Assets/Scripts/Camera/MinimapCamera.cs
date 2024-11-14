using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// ミニマップ作成用カメラ。
/// パフォーマンス改善のために、普段はCameraをDisableにして描画を行っていない。
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

    /// <summary>
    /// ミニマップテクスチャを更新する
    /// 1フレームだけ自分をカメラにしてカメラからテクスチャに書き込む
    /// </summary>
    public void UpdateMinimapTexture()
    {
        //エディター上でEditModeの時は無効化しない
#if UNITY_EDITOR
		if (!EditorApplication.isPlaying) { return; }
#endif
        if (gameObject.activeInHierarchy)
        {
            myCamera.enabled = true;
            StartCoroutine(CrtDisableCameraAfterAFrame());
        }
    }

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

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _minimapMaterial);
    }

    /// <summary>
    /// 1フレーム後にカメラを無効にする
    /// </summary>
    /// <returns></returns>
    private IEnumerator CrtDisableCameraAfterAFrame()
    {
        yield return null;
        myCamera.enabled = false;
    }
}
