using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField, Header("�\���������ʐ�")]
    GameObject canvas;

    //[SerializeField, Header("�S��������")]
    //GameObject allCanvas;

    //[SerializeField, Header("�Q�[���ɖ߂�{�^��")]
    //Button BackButton;

    //[SerializeField, Header("�Z���N�g��ʂɖ߂�{�^��")]
    //Button FinishButton;

    bool gametime;

    /*//�Q�[����ʂɖ߂��
    public void BackClick()
    {
        //�Q�[���I�u�W�F�N�g�\������\��
        allCanvas.SetActive(false);

        //�Q�[���I�u�W�F�N�g��\�����\��
        canvas.SetActive(true);

        gametime = true;
    }

    //�X�e�[�W�Z���N�g��ʂɖ߂��
    public void FinishClick()
    {
        SceneManager.LoadScene("StageSelect");
    }*/
}
