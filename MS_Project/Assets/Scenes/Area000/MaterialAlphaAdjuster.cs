using UnityEngine;
using System.Collections.Generic;

public class MaterialAlphaAdjuster : MonoBehaviour
{
    // 複数のMaterialをリストで指定できるようにする
    public List<Material> targetMaterials = new List<Material>(); // 複数のMaterial
    public float alphaValue = 0.5f; // 透明度の値（0 = 完全透明、1 = 不透明）

    private Renderer objRenderer;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer == null)
        {
            Debug.LogError("Renderer component not found!");
            return;
        }

        // 最初に優先順位の高いMaterialから順に透明度を設定
        SetAlphaValuesInPriority();
    }

    void SetAlphaValuesInPriority()
    {
        if (targetMaterials.Count == 0)
        {
            Debug.LogError("No materials assigned.");
            return;
        }

        // 優先順位に基づいて、リストに登録されたMaterialの透明度を設定
        foreach (Material material in targetMaterials)
        {
            if (material != null)
            {
                Color currentColor = material.color;

                // アルファ値を変更してMaterialに反映
                currentColor.a = alphaValue;
                material.color = currentColor;
            }
            else
            {
                Debug.LogWarning("One of the materials is null, skipping.");
            }
        }
    }
}
