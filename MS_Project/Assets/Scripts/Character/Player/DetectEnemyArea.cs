using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵コライダーを検索するエリア
/// </summary>
public class DetectEnemyArea : DetectArea
{
    //PlayerControllerの参照
    PlayerController playerController;

    BoxCollider boxCollider;

    private void Awake()
    {
        base.Awake();

        boxCollider = GetComponent<BoxCollider>();
    }

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;
    }

    private void Update()
    {
        //方向を一致させる仮処理
        Vector3 currentCenter = boxCollider.center;
        if (playerController.SpriteRenderer.flipX)
        {
            currentCenter.x = Mathf.Abs(currentCenter.x) ; 
         
        }
        else
        {
            currentCenter.x = Mathf.Abs(currentCenter.x) * -1;
        }

        boxCollider.center = currentCenter;

    }

   public  bool CheckKillableEnemy()
   {
        foreach (Collider collider in colliders)
        {
            //敵を取得
            EnemyController enemy = collider.GetComponent<EnemyController>();

            //一撃で殺せるかをチェック
            if (enemy != null && enemy.IsKillable)
            {
                return true;
            }
        }

        return false;
   }

}
