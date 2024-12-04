using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    void Update()
    {
        //ゲームの時間を止める
        Time.timeScale = 0;
    }
}
