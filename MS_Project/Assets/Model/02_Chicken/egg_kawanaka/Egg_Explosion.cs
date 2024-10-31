using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg_Explosion : MonoBehaviour
{
    public GameObject impactEffect; // �G�t�F�N�g�v���n�u

    // OnTriggerEnter�̓g���K�[�ɐN���������ɌĂ΂��
    private void OnTriggerEnter(Collider other)
    {
        // �Փ˂����I�u�W�F�N�g�̃^�O���擾
        string tag = other.gameObject.tag;

        // �^�O��"Ground"�܂���"Player"�̏ꍇ
        if (tag == "Ground" || tag == "Player")
        {
            // �G�t�F�N�g�𐶐�
            Instantiate(impactEffect, transform.position, Quaternion.identity);
            Debug.Log("�G�t�F�N�g�Đ�");
            // �Փ˂�����e������
            Destroy(gameObject);
        }


    }
}
