using UnityEngine;

public class AnimThrowEgg : MonoBehaviour
{
    public GameObject eggPrefab; // ������v���n�u
    public Transform spawnPoint; // �v���n�u�̐����ʒu
    public float throwForce = 10f; // �������

    // �A�j���[�V�����C�x���g����Ăяo�����\�b�h
    public void ThrowEgg()
    {
        if (axePrefab != null && spawnPoint != null)
        {
            // �v���n�u�̃C���X�^���X�𐶐�
            GameObject thrownEgg = Instantiate(axePrefab, spawnPoint.position, spawnPoint.rotation);
            // ����������ɗ͂�������
            Rigidbody rb = thrownEgg.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(spawnPoint.forward * throwForce, ForceMode.Impulse);
            }
        }
    }
}
