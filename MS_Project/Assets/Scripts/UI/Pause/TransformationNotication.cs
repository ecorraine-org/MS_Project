using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationNotication : MonoBehaviour
{
    [SerializeField,Header("�ʒmBOX")]
    GameObject transformationnotication;

    [SerializeField, Header("�\������")]
    float displaytime;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //�Q�[���I�u�W�F�N�g��\�����\��
            transformationnotication.SetActive(true);

            //�Q�[���I�u�W�F�N�g�\������\��
            this.gameObject.SetActive(false);
        }
    }
}
