using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �v���C���[�̃��[�h���Ǘ�����r�w�C�r�A
/// </summary>
public class PlayerModeManager : MonoBehaviour
{
    //PlayerController�̎Q��
    PlayerController playerController;

    [SerializeField, Header("���[�h")]
    PlayerMode mode = PlayerMode.Sword;

    private void OnEnable()
    {
        //�C�x���g���o�C���h����
        OnomatoManager.OnModeChangeEvent += ModeChange;
    }

    private void OnDisable()
    {
        //�o�C���h����������
        OnomatoManager.OnModeChangeEvent -= ModeChange;
    }

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;

    }

    /// <summary>
    /// ���[�h�`�F���W
    /// </summary>
    private void ModeChange(PlayerMode _mode)
    {
        mode = _mode;
        Debug.Log("Manager: ���[�h�`�F���W����" + mode);
    }

    public PlayerMode Mode
    {
        get => this.mode;
        set { this.mode = value; }
    }
}
