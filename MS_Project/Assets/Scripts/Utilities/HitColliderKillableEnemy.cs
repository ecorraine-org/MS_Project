using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitColliderKillableEnemy : HitCollider
{
    [SerializeField, Header("所有者オブジェクト")]
    GameObject owner;

    [SerializeField, NonEditable, Header("一番近い殺せる敵")]
    Collider killableCollider = null;

    //プレイヤーコンポーネントを格納する
    PlayerController player;

    //一番近いコライダーとの距離
    float closestDistanceTokillableCollider = Mathf.Infinity;

    private void Awake()
    {
        player = owner.GetComponentInChildren<PlayerController>();
    }

    protected override void Update()
    {
        base.Update();

        HandleSelectedCollider();
    }


    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        //選ばれたコライダーのチェック
        if (killableCollider == other)
        {
            killableCollider = null;
        }


    }

    public void HandleSelectedCollider()
    {
        //初期化
        closestDistanceTokillableCollider = Mathf.Infinity;

        Vector3 ownerPos = owner.transform.position;

        foreach (var collider in collidersList)
        {
            if (collider == null) continue;

            Vector3 colliderPos = collider.transform.position;

            Vector3 ownerToCollider = colliderPos - ownerPos;
            ownerToCollider.y = 0;

            //殺せる敵の処理
            EnemyController enemy = collider.GetComponentInChildren<EnemyController>();
            if (enemy != null && enemy.Status.CurrentHealth > 0 && enemy.Status.IsKillable && closestDistanceTokillableCollider > ownerToCollider.magnitude)
            {
                closestDistanceTokillableCollider = ownerToCollider.magnitude;
                killableCollider = collider;

              
                //フィニッシャー発動可能にする
                player.SkillManager.CanFinish = true;

            }
        }
    }

    public Collider KillableCollider
    {
        get => killableCollider;
    }
}

