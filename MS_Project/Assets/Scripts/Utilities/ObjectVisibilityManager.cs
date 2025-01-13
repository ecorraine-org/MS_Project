using UnityEngine;

public class ObjectVisibilityManager : MonoBehaviour
{
    private Renderer objectRenderer;
    private Rigidbody objectRigidbody;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // カメラに映っていない場合、非アクティブ化
        if (!objectRenderer.isVisible)
        {
            gameObject.SetActive(false);

            // 物理演算を停止する (Rigidbodyが存在する場合)
            if (objectRigidbody != null)
            {
                objectRigidbody.isKinematic = true;
            }
        }
        else
        {
            gameObject.SetActive(true);

            // 物理演算を再有効化
            if (objectRigidbody != null)
            {
                objectRigidbody.isKinematic = false;
            }
        }
    }
}
