using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationNotication : MonoBehaviour
{
    [SerializeField,Header("�ʒmBOX")]
    GameObject transformationnotication;

    float cunt;

    //�t���[����60�ɌŒ�
    private void Start()
    {
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
