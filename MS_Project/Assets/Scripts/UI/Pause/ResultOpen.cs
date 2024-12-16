using UnityEngine;

public class ResultOpen : MonoBehaviour
{
    public GameObject prefab; // インスペクターで設定するプレハブ
    private GameObject instantiatedPrefab; // 生成されたプレハブを管理

    void Start()
    {
        // 初期状態でプレハブを非アクティブにしておく（必要に応じて）
        if (prefab != null)
        {
            prefab.SetActive(false);
        }
    }

    void Update()
    {
        // 1キーと3キーが同時押しされた場合にプレハブを表示
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha3))
        {
            // まだプレハブが表示されていない場合のみ処理
            if (instantiatedPrefab == null)
            {
                OpenPrefab();
            }
        }
    }

    void OpenPrefab()
    {
        // prefabがインスペクターで設定されているか確認
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned in the inspector!");
            return;
        }

        // プレハブをインスタンス化して表示
        instantiatedPrefab = Instantiate(prefab, transform.position, Quaternion.identity); // プレハブをシーン内に表示
        instantiatedPrefab.SetActive(true); // インスタンスをアクティブにする
        Debug.Log("Prefab opened!");
    }
}
