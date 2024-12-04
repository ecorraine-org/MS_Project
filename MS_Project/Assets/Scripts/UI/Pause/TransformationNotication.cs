using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationNotication : MonoBehaviour
{
    [SerializeField,Header("通知BOX")]
    GameObject transformationnotication;

    [SerializeField, Header("表示時間")]
    float displaytime;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //ゲームオブジェクト非表示→表示
            transformationnotication.SetActive(true);

            //ゲームオブジェクト表示→非表示
            this.gameObject.SetActive(false);
        }
    }
}
