using UnityEngine;

public class AnimThrowAxe : MonoBehaviour
{
    public GameObject axePrefab; // ������v���n�u
    public Transform spawnPoint; // �v���n�u�̐����ʒu
    public float throwForce = 10f; // �������

    // �A�j���[�V�����C�x���g����Ăяo�����\�b�h
    public void ThrowAxe()
    {
        if (axePrefab != null && spawnPoint != null)
        {
            // �v���n�u�̃C���X�^���X�𐶐�
            GameObject thrownAxe = Instantiate(axePrefab, spawnPoint.position, spawnPoint.rotation);
            // ����������ɗ͂�������
            Rigidbody rb = thrownAxe.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(spawnPoint.forward * throwForce, ForceMode.Impulse);
            }
        }
    }
}
