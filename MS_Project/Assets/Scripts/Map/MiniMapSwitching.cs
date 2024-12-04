using UnityEngine;

public class MiniMapSwitching : MonoBehaviour
{
    [SerializeField] private GameObject[] group1; // 最初の配列
    [SerializeField] private GameObject[] group2; // 2つ目の配列

    private bool isGroup1Active = true; // 現在どちらがアクティブかを記録

    private void Update()
    {
        // Mキーが押されたら切り替え
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleGroups();
        }
    }

    private void ToggleGroups()
    {
        // 現在の状態に応じて切り替え
        isGroup1Active = !isGroup1Active;

        // 配列1のアクティブ状態を設定
        foreach (GameObject obj in group1)
        {
            if (obj != null) obj.SetActive(isGroup1Active);
        }

        // 配列2のアクティブ状態を設定
        foreach (GameObject obj in group2)
        {
            if (obj != null) obj.SetActive(!isGroup1Active);
        }
    }
}
