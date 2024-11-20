using System.Collections;  // IEnumerator���g�p���邽�߂ɕK�v
using UnityEngine;

public class DisableColliderTemporarily : MonoBehaviour
{
    private MeshCollider meshCollider;

    void Start()
    {
        // MeshCollider�R���|�[�l���g���擾
        meshCollider = GetComponent<MeshCollider>();

        if (meshCollider != null)
        {
            // 0.5�b��ɃR���C�_�[��L���ɖ߂�
            StartCoroutine(DisableColliderForSeconds(0.5f));
        }
    }

    private IEnumerator DisableColliderForSeconds(float duration)
    {
        // MeshCollider�𖳌���
        meshCollider.enabled = false;

        // �w�莞�Ԃ����҂�
        yield return new WaitForSeconds(duration);

        // MeshCollider���ēx�L����
        meshCollider.enabled = true;
    }
}
