using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariableBinder : MonoBehaviour
{
    // 外部から設定するクラスの例
    [Serializable]
    public class DataClass
    {
        public string variableName;
        public string value;
    }

    // 設定する変数リスト
    public List<DataClass> variables;

    // UIのプレハブ
    public GameObject uiPrefab;

    // UIのコンテナ
    public Transform uiContainer;

    void Start()
    {
        // 変数ごとにUIを生成
        foreach (var variable in variables)
        {
            GameObject uiElement = Instantiate(uiPrefab, uiContainer);
            Text[] textComponents = uiElement.GetComponentsInChildren<Text>();

            if (textComponents.Length >= 2)
            {
                textComponents[0].text = variable.variableName; // ラベル
                textComponents[1].text = variable.value;        // 値
            }
        }
    }
}
