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


   // [SerializeField, NonEditable, Header("一番近い殺せる敵")]
   //Collider killableCollider = null;

    //一番近いコライダーとの距離
    float closestDistance = Mathf.Infinity;
   // float closestDistanceTokillableCollider = Mathf.Infinity;

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

        //if (killableCollider == other)
        //{
        //    killableCollider = null;
        //}


    }

    public void HandleSelectedCollider()
    {
        //初期化
        closestDistance = Mathf.Infinity;
       // closestDistanceTokillableCollider = Mathf.Infinity;

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

            //殺せる敵の処理
            //EnemyController enemy = collider.GetComponentInChildren<EnemyController>();
            //if (enemy !=null && enemy.Status.CurrentHealth>0 && enemy.Status.IsKillable && closestDistanceTokillableCollider > ownerToCollider.magnitude)
            //{

            //    closestDistanceTokillableCollider = ownerToCollider.magnitude;
            //    killableCollider = collider;

            //    //仮処理FinishTest
            //    PlayerController player = owner.GetComponentInChildren<PlayerController>();
            //    player.canFinish = true;

            //}
        }
    }

    public Collider ClosestCollider
    {
        get => closestCollider;
    }

    //public Collider KillableCollider
    //{
    //    get => killableCollider;
    //}
}

