using UnityEngine;

public class ScatterChildren : MonoBehaviour
{
    public float scatterForce = 5f; // 飛び散る力
    public float scatterDuration = 1f; // 飛び散る時間
    public float minGravityScale = 0.5f; // 重力の最小スケール
    public float maxGravityScale = 2f; // 重力の最大スケール
    public float returnDelay = 5f; // 元の位置に戻るまでの待機時間
    public float returnDuration = 5f; // 元の位置に戻るのにかかる時間
    public bool returnToOriginal = true; // 元に戻るかどうかを選択
    public float fadeDuration = 2f; // フェードアウトにかかる時間

    // Start is called before the first frame update
    void Start()
    {
        Scatter();
    }

    void Scatter()
    {
        foreach (Transform child in transform)
        {
            // 子オブジェクトにRigidbodyを追加
            Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();

            // ランダムな方向を生成
            Vector3 scatterDirection = Random.onUnitSphere;

            // 力を加える
            rb.AddForce(scatterDirection * scatterForce, ForceMode.Impulse);

            // ランダムな重力スケールを設定
            float randomGravityScale = Random.Range(minGravityScale, maxGravityScale);
            rb.useGravity = true;
            rb.mass = randomGravityScale;

            // 子オブジェクトの元の位置と回転を保存
            Vector3 originalPosition = child.position;
            Quaternion originalRotation = child.rotation;

            // 飛び散る時間を設定し、その後に元の位置に戻る処理を開始
            Destroy(rb, scatterDuration);

            // 元に戻るかどうかを確認
            if (returnToOriginal)
            {
                StartCoroutine(ReturnToOriginalPosition(child, originalPosition, originalRotation, returnDelay, returnDuration, rb));
            }
            else
            {
                // フェードアウトして消す
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

        Destroy(child.gameObject); // 最終的にオブジェクトを削除
    }
}
