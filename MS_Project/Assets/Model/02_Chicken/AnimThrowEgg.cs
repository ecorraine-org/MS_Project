using UnityEngine;

public class AnimThrowEgg : MonoBehaviour
{
    public GameObject eggPrefab;         // ������v���n�u
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
            InvokeRepeating(nameof(ThrowEgg), 0f, throwInterval);
        }
    }

    // ��������
    private void ThrowEgg()
    {
        if (currentThrow < throwCount)
        {
            if (eggPrefab != null && spawnPoint != null)
            {
                // �v���n�u�̃C���X�^���X�𐶐�
                GameObject thrownEgg = Instantiate(eggPrefab, spawnPoint.position, spawnPoint.rotation);
                // ����������ɗ͂�������
                Rigidbody rb = thrownEgg.GetComponent<Rigidbody>();
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
            CancelInvoke(nameof(ThrowEgg));
            isThrowing = false;

        }
    }
}
