using UnityEngine;

public class MaterialRemoverByName : MonoBehaviour
{
    // �ΏۂƂȂ�GameObject��Renderer
    public Renderer targetRenderer;

    // �폜������Material�̖��O
    public string materialNameToRemove;

    void RemoveMaterialByName()
    {
        if (targetRenderer == null)
        {
            Debug.LogError("�^�[�Q�b�g��Renderer���w�肳��Ă��܂���B");
            return;
        }

        Material[] materials = targetRenderer.materials;
        int indexToRemove = -1;

        // ���O��Material������
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].name.Contains(materialNameToRemove)) // ���O��������v����ꍇ
            {
                indexToRemove = i;
                break;
            }
        }

        if (indexToRemove == -1)
        {
            Debug.LogWarning($"�w�肳�ꂽ���O \"{materialNameToRemove}\" ��Material��������܂���B");
            return;
        }

        // Material���폜���A���X�g���X�V
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

        // �X�V���ꂽMaterial�z���ݒ�
        targetRenderer.materials = newMaterials;

        Debug.Log($"�w�肳�ꂽ���O \"{materialNameToRemove}\" ��Material���폜���܂����B");
    }

    // �f�o�b�O�p�ɃL�[����������Material���폜
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // "R"�L�[�Ŏ��s
        {
            RemoveMaterialByName();
        }
    }
}
