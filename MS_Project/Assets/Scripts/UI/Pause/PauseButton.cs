using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField, Header("表示させる画面先")]
    GameObject canvas;

    //[SerializeField, Header("全部消す時")]
    //GameObject allCanvas;

    //[SerializeField, Header("ゲームに戻るボタン")]
    //Button BackButton;

    //[SerializeField, Header("セレクト画面に戻るボタン")]
    //Button FinishButton;

    bool gametime;

    /*//ゲーム画面に戻る方
    public void BackClick()
    {
        //ゲームオブジェクト表示→非表示
        allCanvas.SetActive(false);

        //ゲームオブジェクト非表示→表示
        canvas.SetActive(true);

        gametime = true;
    }

    //ステージセレクト画面に戻る方
    public void FinishClick()
    {
        SceneManager.LoadScene("StageSelect");
    }*/
}
