using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInputHandler : MonoBehaviour
{

    [SerializeField] private CameraController _cameraController;
    private void Update()
    {
        //Pボタンを押すとホワイトアウトエフェクトが発動する
        if (Input.GetKeyDown(KeyCode.P))
        {
            //_cameraController.ExecuteEffect("WhiteoutEffect");
        }
    }
}
