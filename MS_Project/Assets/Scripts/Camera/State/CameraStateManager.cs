using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateManager : MonoBehaviour
{
    private CameraController _cameraController;

    public void Init(CameraController cameraController)
    {
        _cameraController = cameraController;
    }
}
