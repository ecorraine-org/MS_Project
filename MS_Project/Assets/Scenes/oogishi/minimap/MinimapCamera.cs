//using System.Collections;
//using UnityEngine;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

<<<<<<<< Updated upstream:MS_Project/Assets/Scenes/oogishi/minimap/MinimapCamera.cs
/// <summary>
/// �~�j�}�b�v�쐬�p�J�����B
/// Player�ɒǏ]���Ȃ���~�j�}�b�v�e�N�X�`����`�悵�܂��B
/// </summary>
[ExecuteInEditMode]
public class MinimapCamera : MonoBehaviour
{
    /// <summary>
    /// �����̃J�������擾����
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

    [SerializeField, Tooltip("�Ǐ]�Ώۂ�Player�I�u�W�F�N�g")]
    private Transform _player;

    [SerializeField, Tooltip("�J������Player�̃I�t�Z�b�g")]
    private Vector3 _offset = new Vector3(0, 10, 0);

    [SerializeField, Tooltip("�~�j�}�b�v�ɓK�p����}�e���A��")]
    private Material _minimapMaterial;
========
///// <summary>
///// ミニマップ作成用カメラ。
///// Playerに追従しながらミニマップテクスチャを描画します。
///// </summary>
//[ExecuteInEditMode]
//public class MinimapCamera : MonoBehaviour
//{
//    /// <summary>
//    /// 自分のカメラを取得する
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

//    [SerializeField, Tooltip("追従対象のPlayerオブジェクト")]
//    private Transform _player;

//    [SerializeField, Tooltip("カメラとPlayerのオフセット")]
//    private Vector3 _offset = new Vector3(0, 10, 0);

//    [SerializeField, Tooltip("ミニマップに適用するマテリアル")]
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
    /// �~�j�}�b�v�e�N�X�`�����X�V����
    /// </summary>
    public void UpdateMinimapTexture()
    {
        // �G�f�B�^�[���EditMode�̎��͖��������Ȃ�
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
//    /// ミニマップテクスチャを更新する
//    /// </summary>
//    public void UpdateMinimapTexture()
//    {
//        // エディター上でEditModeの時は無効化しない
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
    /// Player�ɒǏ]����
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
//    /// Playerに追従する
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
