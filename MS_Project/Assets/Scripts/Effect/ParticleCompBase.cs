using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パーティクルのイベント送信先ビヘイビア
/// </summary>
public class ParticleCompBase : MonoBehaviour
{
    protected ParticleSystem particle;

    protected virtual void Awake()
    {
        this.particle = this.GetComponent<ParticleSystem>();
    }


}
