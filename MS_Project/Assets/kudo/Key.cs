using UnityEngine;

public class Key : MonoBehaviour
{
    // デバック用キー入力変化

    // アクティブにしたいオブジェクトをInspectorから設定
    public GameObject targetObject;

    // 使用するキーをpublicにしてInspectorで変更できるようにする。ノーマルはMキー
    public KeyCode activationKey = KeyCode.M;

    void Update()
    {
        // 指定されたキーが押されたとき
        if (Input.GetKeyDown(activationKey))
        {
            // 指定されたオブジェクトをアクティブにする
            if (targetObject != null)
            {
                targetObject.SetActive(true);
                //
                Debug.Log($"{activationKey}キーが押され、{targetObject.name}がアクティブ化");
            }
            else
            {
                Debug.LogWarning("targetObjectが存在しない、もしくは設定していない");
            }
        }
    }
}
