using UnityEngine;
using UnityEditor;

public class RenameToolWindow : EditorWindow
{
    private string baseName = "NewName";  // 名前の基礎を入力するフィールド

    [MenuItem("Tools/Rename Tool")]
    public static void ShowWindow()
    {
        // ウィンドウを表示
        GetWindow<RenameToolWindow>("Rename Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("オブジェクトの名前を一括変更", EditorStyles.boldLabel);

        // 名前入力フィールド
        baseName = EditorGUILayout.TextField("新しい名前の基礎", baseName);

        if (GUILayout.Button("Rename"))
        {
            // 名前入力があるかチェックしてからリネーム処理を実行
            if (!string.IsNullOrEmpty(baseName))
            {
                RenameSelectedObjects(baseName);
            }
            else
            {
                EditorUtility.DisplayDialog("エラー", "新しい名前を入力してください。", "OK");
            }
        }
    }

    private void RenameSelectedObjects(string baseName)
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("オブジェクトが選択されていません。");
            return;
        }

        for (int i = 0; i < selectedObjects.Length; i++)
        {
            selectedObjects[i].name = baseName + "_" + i;  // 連番付きの名前に変更
        }

        Debug.Log("選択されたオブジェクトの名前を一括変更しました。");
    }
}
