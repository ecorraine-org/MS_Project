using UnityEngine;

public class AnimThrowEgg : MonoBehaviour
{
    public GameObject axePrefab;         // ������v���n�u
    public Transform spawnPoint;         // �v���n�u�̐����ʒu
    public float throwForce = 10f;       // �������
    public int throwCount = 3;           // �A�����ē�����񐔁ipublic�Œ����\�j
    public float throwInterval = 0.5f;   // �����̊Ԋu�i�b�j

    private int currentThrow = 0;        // ���݂̓�����
    private bool isThrowing = false;     // �������t���O

    // �A�j���[�V�����C�x���g����Ăяo�����\�b�h
    public void StartThrowSequence()
    {
        if (!isThrowing)
        {
            currentThrow = 0;
            isThrowing = true;
            InvokeRepeating(nameof(ThrowAxe), 0f, throwInterval);
        }
    }

    // ��������
    private void ThrowAxe()
    {
        if (currentThrow < throwCount)
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
            currentThrow++;
        }
        else
        {
            // �������w��񐔂ɒB������I��
            CancelInvoke(nameof(ThrowAxe));
            isThrowing = false;
        }
    }
}
