using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnomatoManager : MonoBehaviour
{
    // ���[�h�`�F���W�C�x���g�̃f���Q�[�g��`
    public delegate void OnomatoEventHandler(PlayerMode mode);

    //  ���[�h�`�F���W�̃C�x���g��`
    public static event OnomatoEventHandler OnModeChangeEvent;

    private void OnEnable()
    {
        //�C�x���g���o�C���h����
        AttackColliderManager.OnOnomatoEvent += Absorb;
    }

    private void OnDisable()
    {
        //�o�C���h����������
        AttackColliderManager.OnOnomatoEvent -= Absorb;
    }

    /// <summary>
    /// �H�ׂ��鏈��
    /// </summary>
    private void Absorb()
    {
        Debug.Log("OnomatoManager:�C�x���g����M�A���[�h�`�F���W");
        //���[�h�`�F���W�̃C�x���g���M
        OnModeChangeEvent?.Invoke(PlayerMode.Sword);
    }
}
