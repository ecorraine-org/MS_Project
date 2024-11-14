using UnityEngine;

public class ScatterChildren : MonoBehaviour
{
    public float scatterForce = 5f; // ��юU���
    public float scatterDuration = 1f; // ��юU�鎞��
    public float minGravityScale = 0.5f; // �d�͂̍ŏ��X�P�[��
    public float maxGravityScale = 2f; // �d�͂̍ő�X�P�[��
    public float returnDelay = 5f; // ���̈ʒu�ɖ߂�܂ł̑ҋ@����
    public float returnDuration = 5f; // ���̈ʒu�ɖ߂�̂ɂ����鎞��
    public bool returnToOriginal = true; // ���ɖ߂邩�ǂ�����I��
    public float fadeDuration = 2f; // �t�F�[�h�A�E�g�ɂ����鎞��

    // Start is called before the first frame update
    void Start()
    {
        Scatter();
    }

    void Scatter()
    {
        foreach (Transform child in transform)
        {
            // �q�I�u�W�F�N�g��Rigidbody��ǉ�
            Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();

            // �����_���ȕ����𐶐�
            Vector3 scatterDirection = Random.onUnitSphere;

            // �͂�������
            rb.AddForce(scatterDirection * scatterForce, ForceMode.Impulse);

            // �����_���ȏd�̓X�P�[����ݒ�
            float randomGravityScale = Random.Range(minGravityScale, maxGravityScale);
            rb.useGravity = true;
            rb.mass = randomGravityScale;

            // �q�I�u�W�F�N�g�̌��̈ʒu�Ɖ�]��ۑ�
            Vector3 originalPosition = child.position;
            Quaternion originalRotation = child.rotation;

            // ��юU�鎞�Ԃ�ݒ肵�A���̌�Ɍ��̈ʒu�ɖ߂鏈�����J�n
            Destroy(rb, scatterDuration);

            // ���ɖ߂邩�ǂ������m�F
            if (returnToOriginal)
            {
                StartCoroutine(ReturnToOriginalPosition(child, originalPosition, originalRotation, returnDelay, returnDuration, rb));
            }
            else
            {
                // �t�F�[�h�A�E�g���ď���
                StartCoroutine(FadeOutAndDestroy(child, returnDelay, fadeDuration));
            }
        }
    }

    private System.Collections.IEnumerator ReturnToOriginalPosition(Transform child, Vector3 originalPosition, Quaternion originalRotation, float delay, float duration, Rigidbody rb)
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;
        Vector3 startingPosition = child.position;
        Quaternion startingRotation = child.rotation;

        while (elapsedTime < duration)
        {
            child.position = Vector3.Lerp(startingPosition, originalPosition, (elapsedTime / duration));
            child.rotation = Quaternion.Slerp(startingRotation, originalRotation, (elapsedTime / duration));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        child.position = originalPosition;
        child.rotation = originalRotation;
        StartCoroutine(FadeOutAndDestroy(child, 0f, fadeDuration));
    }

    private System.Collections.IEnumerator FadeOutAndDestroy(Transform child, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        Renderer renderer = child.GetComponent<Renderer>();
        if (renderer == null) yield break;

        Color originalColor = renderer.material.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            renderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(child.gameObject); // �ŏI�I�ɃI�u�W�F�N�g���폜
    }
}
