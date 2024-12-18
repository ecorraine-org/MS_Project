using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField, Header("表示させる画面先")]
    GameObject canvas;

    bool gametime;
    public void Update()
    {
        //Pouse画面の処理
        if (gametime == true)
        {
            //時間経過速度
            Time.timeScale = 1;

            //ゲームオブジェクト表示→非表示
            this.gameObject.SetActive(false);

            //ゲームオブジェクト非表示→表示
            canvas.SetActive(true);

            gametime = false;
        }
        else if (gametime == false && Input.GetKeyDown(KeyCode.Space))
        {
            //時間経過速度
            Time.timeScale = 0;

            //ゲームオブジェクト表示→非表示
            this.gameObject.SetActive(false);

            //ゲームオブジェクト非表示→表示
            canvas.SetActive(true);

            gametime = true;
        }
    }
}