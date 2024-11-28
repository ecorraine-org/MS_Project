using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraStateSettings
{
    public float TransitionSpeed = 1.0f;
    public string DefaultState = "Idle";
    public bool EnableSmoothing = true;
}
