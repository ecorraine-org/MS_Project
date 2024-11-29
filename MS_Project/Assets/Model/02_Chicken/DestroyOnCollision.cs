using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private Transform player;

    // �������iPlayer��Transform��n���j
    public void Init()
    {
        // Player�I�u�W�F�N�g��Hierarchy����擾
        player = GameObject.Find("Player")?.transform; // "Player"�͎��ۂ�Player�I�u�W�F�N�g�̖��O�ɕύX���Ă�������
    }

    // �Փˎ��ɌĂ΂�郁�\�b�h
    private void OnCollisionEnter(Collision collision)
    {
        // Player�ɏՓ˂����ꍇ�A�I�u�W�F�N�g���폜
        if (collision.transform == player)
        {
            Destroy(gameObject); // �������g�i���I�u�W�F�N�g�j���폜
        }
    }
}
