using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField,Header("�\��������Canvas")]
    GameObject canvas;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //�Q�[���I�u�W�F�N�g�\������\��
            this.gameObject.SetActive(false);

            //�Q�[���I�u�W�F�N�g��\�����\��
            canvas.SetActive(true);
         }
    }
}
