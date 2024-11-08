using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DepthTexture : MonoBehaviour
{
    void Start()
    {
        //DepthTextureを指定
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    }

}
