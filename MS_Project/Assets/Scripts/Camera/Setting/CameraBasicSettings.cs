using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraBasicSettings
{
    public float FieldOfView = 60f;
    public float NearClipPlane = 0.3f;
    public float FarClipPlane = 1000f;
    public Vector3 DefaultOffset = new Vector3(0, 2, -5);
    public LayerMask CullingMask;
}
