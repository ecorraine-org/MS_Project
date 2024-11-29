using UnityEngine;
using System.Collections.Generic;

public class MaterialAlphaAdjuster : MonoBehaviour
{
    // ������Material�����X�g�Ŏw��ł���悤�ɂ���
    public List<Material> targetMaterials = new List<Material>(); // ������Material
    public float alphaValue = 0.5f; // �����x�̒l�i0 = ���S�����A1 = �s�����j

    private Renderer objRenderer;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer == null)
        {
            Debug.LogError("Renderer component not found!");
            return;
        }

        // �ŏ��ɗD�揇�ʂ̍���Material���珇�ɓ����x��ݒ�
        SetAlphaValuesInPriority();
    }

    void SetAlphaValuesInPriority()
    {
        if (targetMaterials.Count == 0)
        {
            Debug.LogError("No materials assigned.");
            return;
        }

        // �D�揇�ʂɊ�Â��āA���X�g�ɓo�^���ꂽMaterial�̓����x��ݒ�
        foreach (Material material in targetMaterials)
        {
            if (material != null)
            {
                Color currentColor = material.color;

                // �A���t�@�l��ύX����Material�ɔ��f
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
