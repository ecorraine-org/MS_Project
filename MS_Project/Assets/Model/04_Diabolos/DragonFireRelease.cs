using UnityEngine;

public class DragonFireRelease : MonoBehaviour
{
    public GameObject firePrefab;        // ������v���n�u
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
            InvokeRepeating(nameof(ThrowFire), 0f, throwInterval);
        }
    }

    // ��������
    // �A���Γ�
    private void ThrowFire()
    {
        if (currentThrow < throwCount)
        {
            if (firePrefab != null && spawnPoint != null)
            {
                // �v���n�u�̃C���X�^���X�𐶐�
                GameObject thrownFire = Instantiate(firePrefab, spawnPoint.position, spawnPoint.rotation);
                // ����������ɗ͂�������
                Rigidbody rb = thrownFire.GetComponent<Rigidbody>();
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
            CancelInvoke(nameof(ThrowFire));
            isThrowing = false;
        }
    }

    // �V���O���Γ�
    public void SingleFire()
    {
        if (firePrefab != null && spawnPoint != null)
        {
            // �v���n�u�̃C���X�^���X�𐶐�
            GameObject thrownFire = Instantiate(firePrefab, spawnPoint.position, spawnPoint.rotation);
            // ����������ɗ͂�������
            Rigidbody rbFire = thrownFire.GetComponent<Rigidbody>();
            if (rbFire != null)
            {
                rbFire.AddForce(spawnPoint.forward * throwForce, ForceMode.Impulse);
            }
        }
    }
}