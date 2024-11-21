using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _minimapMaterial);
    }

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
