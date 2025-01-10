using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField, Header("表示させるUI")]
    GameObject pauseUI;

    bool gametime;

    enum PlayState
    {
        None,
        Playing,
        Paused
    }
    PlayState playState = PlayState.None;

    public void Start()
    {
        playState = PlayState.Playing;

        if (pauseUI.activeInHierarchy)
            pauseUI.SetActive(false);
    }

    public void Update()
    {
        //Pause画面の処理
        switch (playState)
        {
            case PlayState.Playing:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Time.timeScale = 0; //時間経過速度
                    pauseUI.SetActive(true);
                    playState = PlayState.Paused;
                }
                break;
            case PlayState.Paused:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Time.timeScale = 1;
                    pauseUI.SetActive(false);
                    playState = PlayState.Playing;
                }
                break;
        }
        /*
        if (gametime == true)
        {
            //時間経過速度
            Time.timeScale = 1;

            //ゲームオブジェクト表示→非表示
            this.gameObject.SetActive(false);

            //ゲームオブジェクト非表示→表示
            //canvas.SetActive(true);

            gametime = false;
        }
        else if (gametime == false && Input.GetKeyDown(KeyCode.Space))
        {
            //時間経過速度
            Time.timeScale = 0;

            //ゲームオブジェクト表示→非表示
            this.gameObject.SetActive(true);

            //ゲームオブジェクト非表示→表示
            //canvas.SetActive(true);

            gametime = true;
        }
        */
    }
}