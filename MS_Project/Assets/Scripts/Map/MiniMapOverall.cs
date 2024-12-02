using UnityEngine;

public class MiniMapOverall : MonoBehaviour
{
    [SerializeField] private Transform player; // �v���C���[��Transform��ݒ�
    [SerializeField] private Vector3 offset = new Vector3(0, 50, 0); // �~�j�}�b�v�J�����̈ʒu����

    private void LateUpdate()
    {
        if (player != null)
        {
            // �v���C���[�̈ʒu�ɃI�t�Z�b�g�������ăJ�������ړ�
            transform.position = player.position + offset;
        }
    }
}
