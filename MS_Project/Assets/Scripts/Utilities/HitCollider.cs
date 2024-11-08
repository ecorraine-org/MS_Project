﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitCollider : MonoBehaviour
{
    [SerializeField, Header("コライダーリスト")]
    private List<Collider> collidersList = new List<Collider>();

 
    private void OnTriggerEnter(Collider other)
    {
        // 破壊されたオブジェクトをリストから削除
       collidersList.RemoveAll(item => item == null);

        if (!collidersList.Contains(other))
        {
            collidersList.Add(other);

        }
    }


    private void OnTriggerExit(Collider other)
    {
        // 破壊されたオブジェクトをリストから削除
        collidersList.RemoveAll(item => item == null);

        if (collidersList.Contains(other))
        {
            collidersList.Remove(other);
        }

    }


    public List<Collider> CollidersList
    {
        get => collidersList;
    }
}