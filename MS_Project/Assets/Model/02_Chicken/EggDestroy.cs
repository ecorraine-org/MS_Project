using UnityEngine;

public class ThrownEgg : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // "Player" �^�O�̃I�u�W�F�N�g�ɐڐG�����ꍇ�ɍ폜
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
