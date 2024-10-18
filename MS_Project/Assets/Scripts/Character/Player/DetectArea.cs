using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コライダー検索エリア
/// </summary>
public class DetectArea : MonoBehaviour
{
    [SerializeField, Header("ターゲットにするコライダー一覧")]
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
