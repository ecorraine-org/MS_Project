using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パーティクルの再生終了時に自動削除するためのビヘイビア
/// </summary>
public class DestroyParticle : MonoBehaviour
{
    ParticleSystem particle;

    private void Awake()
    {
        this.particle = this.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (this.particle.isPlaying) return;
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 子オブジェクト含めて、サイズ変更
    /// </summary>
    public void ChangeScale(float _size)
    {
        transform.localScale *= _size;

        ChangeAllChildrenScale(transform, _size);
    }


    /// <summary>
    /// 再帰で子オブジェクトサイズ変更
    /// </summary>
    private void ChangeAllChildrenScale(Transform _parentTransform, float _size)
    {
        foreach (Transform childTransform in _parentTransform)
        {
            childTransform.localScale *= _size;

            ChangeAllChildrenScale(childTransform, _size);
        }
    }
}
