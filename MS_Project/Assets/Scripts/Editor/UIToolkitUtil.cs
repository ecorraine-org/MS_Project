using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace StageEditor
{
    public class UIToolkitUtil : MonoBehaviour
    {
        private const string k_UxmlPath = "Assets/StageEditor/Editor/StageEditorWindow.uxml";

        public static VisualTreeAsset GetVisualTree(string path)
        {
            var fullPath = k_UxmlPath + path + ".uxml";
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(fullPath);
        }
    }
}
