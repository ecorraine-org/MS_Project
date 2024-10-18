using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �R���C�_�[�����G���A
/// </summary>
public class DetectArea : MonoBehaviour
{
    [SerializeField, Header("�^�[�Q�b�g�ɂ���R���C�_�[�ꗗ")]
    protected List<Collider> colliders = new List<Collider>();

    protected Collider collider;

    protected void Awake()
    {
        collider = GetComponent<Collider>();
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!colliders.Contains(other))
        {
            colliders.Add(other);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (colliders.Contains(other))
        {
            colliders.Remove(other);
        }
    }



}
