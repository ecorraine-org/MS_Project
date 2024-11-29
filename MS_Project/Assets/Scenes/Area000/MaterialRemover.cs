using UnityEngine;

public class MaterialRemoverByName : MonoBehaviour
{
    // 対象となるGameObjectのRenderer
    public Renderer targetRenderer;

    // 削除したいMaterialの名前
    public string materialNameToRemove;

    void RemoveMaterialByName()
    {
        if (targetRenderer == null)
        {
            Debug.LogError("ターゲットのRendererが指定されていません。");
            return;
        }

        Material[] materials = targetRenderer.materials;
        int indexToRemove = -1;

        // 名前でMaterialを検索
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].name.Contains(materialNameToRemove)) // 名前が部分一致する場合
            {
                indexToRemove = i;
                break;
            }
        }

        if (indexToRemove == -1)
        {
            Debug.LogWarning($"指定された名前 \"{materialNameToRemove}\" のMaterialが見つかりません。");
            return;
        }

        // Materialを削除し、リストを更新
        Material[] newMaterials = new Material[materials.Length - 1];
        int newIndex = 0;

        for (int i = 0; i < materials.Length; i++)
        {
            if (i != indexToRemove)
            {
                newMaterials[newIndex] = materials[i];
                newIndex++;
            }
        }

        // 更新されたMaterial配列を設定
        targetRenderer.materials = newMaterials;

        Debug.Log($"指定された名前 \"{materialNameToRemove}\" のMaterialを削除しました。");
    }

    // デバッグ用にキーを押したらMaterialを削除
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // "R"キーで実行
        {
            RemoveMaterialByName();
        }
    }
}
