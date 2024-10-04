using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderManager : MonoBehaviour
{
    Collider[] hitColliders;

    /// <summary>
    /// �R���C�_�[�̌��o���s���A�ΏۂɃ_���[�W��^����
    /// </summary>
    public void DetectColliders(Vector3 _pos, Vector3 _size, float _damage, LayerMask _targetLayer)
    {
        int omomatoLayer = LayerMask.NameToLayer("Onomatopoeia");

        hitColliders = Physics.OverlapBox(_pos, _size / 2, UnityEngine.Quaternion.identity, _targetLayer);

        if (hitColliders.Length <= 0) return;


        foreach (Collider hitCollider in hitColliders)
        {
            var life = hitCollider.GetComponentInChildren<ILife>();
            if (life != null)
            {
                life.TakeDamage(_damage);
            }

            //���̃I�m�}�g�y����
            if (hitCollider.gameObject.layer == omomatoLayer)
            {
                Debug.Log("ONOMATOPE ");
                //���b�Z�[�W
                Destroy(hitCollider.gameObject);
            }
        }
    }

    public Collider[] HitColliders
    {
        get => this.hitColliders;
        set { this.hitColliders = value; }
    }

}
