using System.Collections;  // IEnumeratorを使用するために必要
using UnityEngine;

public class DisableColliderTemporarily : MonoBehaviour
{
    private MeshCollider meshCollider;

    void Start()
    {
        // MeshColliderコンポーネントを取得
        meshCollider = GetComponent<MeshCollider>();

        if (meshCollider != null)
        {
            // 0.5秒後にコライダーを有効に戻す
            StartCoroutine(DisableColliderForSeconds(0.5f));
        }
    }

    private IEnumerator DisableColliderForSeconds(float duration)
    {
        // MeshColliderを無効化
        meshCollider.enabled = false;

        // 指定時間だけ待つ
        yield return new WaitForSeconds(duration);

        // MeshColliderを再度有効化
        meshCollider.enabled = true;
    }
}
