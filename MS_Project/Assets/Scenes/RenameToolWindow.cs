using UnityEngine;
using UnityEditor;

public class RenameToolWindow : EditorWindow
{
    private string baseName = "NewName";  // ���O�̊�b����͂���t�B�[���h

    [MenuItem("Tools/Rename Tool")]
    public static void ShowWindow()
    {
        // �E�B���h�E��\��
        GetWindow<RenameToolWindow>("Rename Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("�I�u�W�F�N�g�̖��O���ꊇ�ύX", EditorStyles.boldLabel);

        // ���O���̓t�B�[���h
        baseName = EditorGUILayout.TextField("�V�������O�̊�b", baseName);

        if (GUILayout.Button("Rename"))
        {
            // ���O���͂����邩�`�F�b�N���Ă��烊�l�[�����������s
            if (!string.IsNullOrEmpty(baseName))
            {
                RenameSelectedObjects(baseName);
            }
            else
            {
                EditorUtility.DisplayDialog("�G���[", "�V�������O����͂��Ă��������B", "OK");
            }
        }
    }

    private void RenameSelectedObjects(string baseName)
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("�I�u�W�F�N�g���I������Ă��܂���B");
            return;
        }

        for (int i = 0; i < selectedObjects.Length; i++)
        {
            selectedObjects[i].name = baseName + "_" + i;  // �A�ԕt���̖��O�ɕύX
        }

        Debug.Log("�I�����ꂽ�I�u�W�F�N�g�̖��O���ꊇ�ύX���܂����B");
    }
}
