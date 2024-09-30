using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderManager : MonoBehaviour
{
    Collider[] hitColliders;

    /// <summary>
    /// コライダーの検出を行い、対象にダメージを与える
    /// </summary>
    /// <param name="_offset">攻撃エリアのオフセット位置</param>
    public void DetectColliders(Vector3 _pos, Vector3 _offset, Vector3 _size, float _damage, LayerMask _targetLayer)
    {
        hitColliders = Physics.OverlapBox(_pos, _size / 2, UnityEngine.Quaternion.identity, _targetLayer);

        if (hitColliders.Length <= 0) return;

        foreach (Collider hitCollider in hitColliders)
        {
            var life = hitCollider.GetComponentInChildren<ILife>();
            if (life != null)
            {
                life.TakeDamage(_damage);
            }
        }
    }

    public Collider[] HitColliders
    {
        get => this.hitColliders;
        set { this.hitColliders = value; }
    }

}
