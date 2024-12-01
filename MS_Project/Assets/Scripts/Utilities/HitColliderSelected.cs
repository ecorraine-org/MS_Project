using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitColliderSelected : HitCollider
{
    [SerializeField, Header("所有者オブジェクト")]
    GameObject owner;

    [SerializeField, NonEditable,Header("一番近いコライダー")]
    Collider closestCollider = null;

    //一番近いコライダーとの距離
    float closestDistance = Mathf.Infinity;

    protected override void Update()
    {
        base.Update();

        HandleSelectedCollider();
    }


    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        //選ばれたコライダーのチェック
        if (closestCollider == other)
        {
            closestCollider = null;
        }

  
    }

    public void HandleSelectedCollider()
    {
        //初期化
        closestDistance = Mathf.Infinity;

        Vector3 ownerPos = owner.transform.position;

        foreach (var collider in collidersList)
        {
            Vector3 colliderPos = collider.transform.position;

            Vector3 ownerToCollider = colliderPos - ownerPos;
            ownerToCollider.y = 0;

            //一番近いコライダーを取得
            if (closestDistance > ownerToCollider.magnitude)
            {
                closestDistance = ownerToCollider.magnitude;
                closestCollider = collider;
            }
        }
    }

    public Collider ClosestCollider
    {
        get => closestCollider;
    }
}
