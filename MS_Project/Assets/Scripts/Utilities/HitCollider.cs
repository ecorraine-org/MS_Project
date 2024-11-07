using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitCollider : MonoBehaviour
{
    //当たったら送信
    public event Action<Collider> OnCollide;

    [SerializeField, Header("コライダーリスト")]
    private List<Collider> collidersList = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (!collidersList.Contains(other))
        {
            collidersList.Add(other);
            Debug.Log("Enter: " + other.name);

            //当たり判定処理イベント
            OnCollide?.Invoke(other);
        }
    }

  
    private void OnTriggerExit(Collider other)
    {
        if (collidersList.Contains(other))
        {
            collidersList.Remove(other);
            Debug.Log("Exit: " + other.name);
        }
      
    }



    //List<Collider> CollidersList
    //{
    //    get => collidersList;
    //}
}
