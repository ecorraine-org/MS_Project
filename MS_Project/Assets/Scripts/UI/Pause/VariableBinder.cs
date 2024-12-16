using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariableBinder : MonoBehaviour
{
    // �O������ݒ肷��N���X�̗�
    [Serializable]
    public class DataClass
    {
        public string variableName;
        public string value;
    }

    // �ݒ肷��ϐ����X�g
    public List<DataClass> variables;

    // UI�̃v���n�u
    public GameObject uiPrefab;

    // UI�̃R���e�i
    public Transform uiContainer;

    void Start()
    {
        // �ϐ����Ƃ�UI�𐶐�
        foreach (var variable in variables)
        {
            GameObject uiElement = Instantiate(uiPrefab, uiContainer);
            Text[] textComponents = uiElement.GetComponentsInChildren<Text>();

            if (textComponents.Length >= 2)
            {
                textComponents[0].text = variable.variableName; // ���x��
                textComponents[1].text = variable.value;        // �l
            }
        }
    }
}
