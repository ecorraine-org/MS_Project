using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationNotication : MonoBehaviour
{
    [SerializeField,Header("通知BOX")]
    GameObject transformationnotication;

    float cunt;

    private void Start()
    {
        //フレームを60に固定
        //Application.targetFrameRate = 60;
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
            cunt += Time.deltaTime;

            //ゲームオブジェクト非表示→表示
            //transformationnotication.SetActive(true);

            Debug.Log(cunt);

            //4秒後に消す処理
            if (cunt >= 4)
            {
                //ゲームオブジェクト表示→非表示
                transformationnotication.SetActive(false);
            }
        //}
    }
}
