using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationNotication : MonoBehaviour
{
    [SerializeField,Header("�ʒmBOX")]
    GameObject transformationnotication;

    float cunt;

    private void Start()
    {
        //�t���[����60�ɌŒ�
        //Application.targetFrameRate = 60;
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
            cunt += Time.deltaTime;

            //�Q�[���I�u�W�F�N�g��\�����\��
            //transformationnotication.SetActive(true);

            Debug.Log(cunt);

            //4�b��ɏ�������
            if (cunt >= 4)
            {
                //�Q�[���I�u�W�F�N�g�\������\��
                transformationnotication.SetActive(false);
            }
        //}
    }
}
