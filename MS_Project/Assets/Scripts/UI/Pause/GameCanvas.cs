using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField,Header("表示させるCanvas")]
    GameObject canvas;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ゲームオブジェクト表示→非表示
            this.gameObject.SetActive(false);

            //ゲームオブジェクト非表示→表示
            canvas.SetActive(true);
         }
    }
}
