using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInputHandler : MonoBehaviour
{

    [SerializeField] private CameraController _cameraController;
    private CameraEffectManager _cameraEffectManager;
    //Lキーを押すとホワイトアウトエフェクトが発動する
    // [SerializeField] private KeyCode _whiteoutEffectKey = KeyCode.P;

    private void Start()
    {
        _cameraEffectManager = CameraEffectManager.Instance;
    }
    private void Update()
    {

    }
}
