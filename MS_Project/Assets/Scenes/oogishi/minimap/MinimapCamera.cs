//using System.Collections;
//using UnityEngine;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

<<<<<<<< Updated upstream:MS_Project/Assets/Scenes/oogishi/minimap/MinimapCamera.cs
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
========
///// <summary>
///// 繝溘ル繝槭ャ繝嶺ｽ懈�千畑繧ｫ繝｡繝ｩ縲�
///// Player縺ｫ霑ｽ蠕薙＠縺ｪ縺後ｉ繝溘ル繝槭ャ繝励ユ繧ｯ繧ｹ繝√Ε繧呈緒逕ｻ縺励∪縺吶��
///// </summary>
//[ExecuteInEditMode]
//public class MinimapCamera : MonoBehaviour
//{
//    /// <summary>
//    /// 閾ｪ蛻�縺ｮ繧ｫ繝｡繝ｩ繧貞叙蠕励☆繧�
//    /// </summary>
//    public Camera myCamera
//    {
//        get
//        {
//            if (!_myCamera)
//            {
//                _myCamera = GetComponent<Camera>();
//            }
//            return _myCamera;
//        }
//    }

//    [SerializeField, Tooltip("霑ｽ蠕灘ｯｾ雎｡縺ｮPlayer繧ｪ繝悶ず繧ｧ繧ｯ繝�")]
//    private Transform _player;

//    [SerializeField, Tooltip("繧ｫ繝｡繝ｩ縺ｨPlayer縺ｮ繧ｪ繝輔そ繝�繝�")]
//    private Vector3 _offset = new Vector3(0, 10, 0);

//    [SerializeField, Tooltip("繝溘ル繝槭ャ繝励↓驕ｩ逕ｨ縺吶ｋ繝槭ユ繝ｪ繧｢繝ｫ")]
//    private Material _minimapMaterial;
>>>>>>>> Stashed changes:MS_Project/Assets/Minimap/MinimapCamera.cs

//    private Camera _myCamera;

//    private void Awake()
//    {
//        myCamera.depthTextureMode = DepthTextureMode.Depth;
//    }

//    private void OnEnable()
//    {
//        UpdateMinimapTexture();
//    }

//    private void Update()
//    {
//        FollowPlayer();
//    }

<<<<<<<< Updated upstream:MS_Project/Assets/Scenes/oogishi/minimap/MinimapCamera.cs
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
========
//    /// <summary>
//    /// 繝溘ル繝槭ャ繝励ユ繧ｯ繧ｹ繝√Ε繧呈峩譁ｰ縺吶ｋ
//    /// </summary>
//    public void UpdateMinimapTexture()
//    {
//        // 繧ｨ繝�繧｣繧ｿ繝ｼ荳翫〒EditMode縺ｮ譎ゅ�ｯ辟｡蜉ｹ蛹悶＠縺ｪ縺�
//#if UNITY_EDITOR
//        if (!EditorApplication.isPlaying) { return; }
//#endif
//        if (gameObject.activeInHierarchy)
//        {
//            myCamera.enabled = true;
//        }
//    }
>>>>>>>> Stashed changes:MS_Project/Assets/Minimap/MinimapCamera.cs

//    private void OnRenderImage(RenderTexture source, RenderTexture destination)
//    {
//        Graphics.Blit(source, destination, _minimapMaterial);
//    }

<<<<<<<< Updated upstream:MS_Project/Assets/Scenes/oogishi/minimap/MinimapCamera.cs
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
========
//    /// <summary>
//    /// Player縺ｫ霑ｽ蠕薙☆繧�
//    /// </summary>
//    private void FollowPlayer()
//    {
//        if (_player != null)
//        {
//            transform.position = _player.position + _offset;
//        }
//    }
//}
>>>>>>>> Stashed changes:MS_Project/Assets/Minimap/MinimapCamera.cs
