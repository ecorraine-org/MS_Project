using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Throw_Rock : MonoBehaviour
{
    public GameObject rockPrefab; // ������v���n�u
    public Transform spawnPoint; // �v���n�u�̐����ʒu
    public float throwForce = 10f; // �������

    // �A�j���[�V�����C�x���g����Ăяo�����\�b�h
    public void ThrowRock()
    {
        if (rockPrefab != null && spawnPoint != null)
        {
            // �v���n�u�̃C���X�^���X�𐶐�
            GameObject thrownRock = Instantiate(rockPrefab, spawnPoint.position, spawnPoint.rotation);
            // ����������ɗ͂�������
            Rigidbody rb = thrownRock.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(spawnPoint.forward * throwForce, ForceMode.Impulse);
            }
        }
    }
}
