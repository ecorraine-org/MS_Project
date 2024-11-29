//using System.Collections;
//using UnityEngine;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

<<<<<<<< Updated upstream:MS_Project/Assets/Scenes/oogishi/minimap/MinimapCamera.cs
/// <summary>
/// ƒ~ƒjƒ}ƒbƒvì¬—pƒJƒƒ‰B
/// Player‚É’Ç]‚µ‚È‚ª‚çƒ~ƒjƒ}ƒbƒvƒeƒNƒXƒ`ƒƒ‚ğ•`‰æ‚µ‚Ü‚·B
/// </summary>
[ExecuteInEditMode]
public class MinimapCamera : MonoBehaviour
{
    /// <summary>
    /// ©•ª‚ÌƒJƒƒ‰‚ğæ“¾‚·‚é
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

    [SerializeField, Tooltip("’Ç]‘ÎÛ‚ÌPlayerƒIƒuƒWƒFƒNƒg")]
    private Transform _player;

    [SerializeField, Tooltip("ƒJƒƒ‰‚ÆPlayer‚ÌƒIƒtƒZƒbƒg")]
    private Vector3 _offset = new Vector3(0, 10, 0);

    [SerializeField, Tooltip("ƒ~ƒjƒ}ƒbƒv‚É“K—p‚·‚éƒ}ƒeƒŠƒAƒ‹")]
    private Material _minimapMaterial;
========
///// <summary>
///// ãƒŸãƒ‹ãƒãƒƒãƒ—ä½œæˆç”¨ã‚«ãƒ¡ãƒ©ã€‚
///// Playerã«è¿½å¾“ã—ãªãŒã‚‰ãƒŸãƒ‹ãƒãƒƒãƒ—ãƒ†ã‚¯ã‚¹ãƒãƒ£ã‚’æç”»ã—ã¾ã™ã€‚
///// </summary>
//[ExecuteInEditMode]
//public class MinimapCamera : MonoBehaviour
//{
//    /// <summary>
//    /// è‡ªåˆ†ã®ã‚«ãƒ¡ãƒ©ã‚’å–å¾—ã™ã‚‹
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

//    [SerializeField, Tooltip("è¿½å¾“å¯¾è±¡ã®Playerã‚ªãƒ–ã‚¸ã‚§ã‚¯ãƒˆ")]
//    private Transform _player;

//    [SerializeField, Tooltip("ã‚«ãƒ¡ãƒ©ã¨Playerã®ã‚ªãƒ•ã‚»ãƒƒãƒˆ")]
//    private Vector3 _offset = new Vector3(0, 10, 0);

//    [SerializeField, Tooltip("ãƒŸãƒ‹ãƒãƒƒãƒ—ã«é©ç”¨ã™ã‚‹ãƒãƒ†ãƒªã‚¢ãƒ«")]
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
    /// ƒ~ƒjƒ}ƒbƒvƒeƒNƒXƒ`ƒƒ‚ğXV‚·‚é
    /// </summary>
    public void UpdateMinimapTexture()
    {
        // ƒGƒfƒBƒ^[ã‚ÅEditMode‚Ì‚Í–³Œø‰»‚µ‚È‚¢
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
//    /// ãƒŸãƒ‹ãƒãƒƒãƒ—ãƒ†ã‚¯ã‚¹ãƒãƒ£ã‚’æ›´æ–°ã™ã‚‹
//    /// </summary>
//    public void UpdateMinimapTexture()
//    {
//        // ã‚¨ãƒ‡ã‚£ã‚¿ãƒ¼ä¸Šã§EditModeã®æ™‚ã¯ç„¡åŠ¹åŒ–ã—ãªã„
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
    /// Player‚É’Ç]‚·‚é
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
//    /// Playerã«è¿½å¾“ã™ã‚‹
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
