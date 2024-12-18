using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEatState : PlayerState
{
    [SerializeField, Header("コライダー")]
    HitCollider hitCollider;

    //捕食test
    public float attackDamage;
    public LayerMask onomatoLayer;

    [SerializeField, Header("敵レイヤー")]
    private LayerMask enemyLayer;

    //捕食方向
    Vector3 eatingDirec;

    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(true);

        base.Init(_playerController);

        playerController.AttackColliderV2.HitCollidersList = hitCollider;

        //方向変更
       // playerController.SetEightDirection();

        ////入力方向取得
        //UnityEngine.Vector2 inputDirec = inputManager.GetMoveDirec();

        ////捕食方向設定
        //eatingDirec = new Vector3(inputDirec.x, 0, inputDirec.y);

        //暴走時、クールタイムを無視する
        if (playerController.StatusManager.IsFrenzy)
        {
            playerController.SkillManager.InitializeSkill(PlayerSkill.Eat);
        }
        else
        {
            playerController.SkillManager.UseSkill(PlayerSkill.Eat);
        }
       
    }

    public override void Tick()
    {
        Attack();


        

        //ボタンを押している間、オノマトペを選択
        if (playerController.InputManager.GetEatPressed() && playerController.SkillManager.CanCharge)
        {
            playerController.SpriteAnim.speed = 0;

            //時間を遅くする
            Time.timeScale = 0.1f;

            //方向変更
            playerController.SetEightDirection();

            //入力方向取得
            UnityEngine.Vector2 inputDirec = inputManager.GetMoveDirec();
            //捕食方向設定
            eatingDirec = new Vector3(inputDirec.x, 0, inputDirec.y);
            if (inputDirec == Vector2.zero) eatingDirec = playerController.GetForward();

            //コライダーの検出
            playerController.AttackColliderV2.SelectColliderWithInputDirec(playerController.transform, 0.0f, eatingDirec, onomatoLayer);


           

        }

        //ボタンを離す
        if(!playerController.InputManager.GetEatPressed())
        {
            if (playerController.SkillManager.CanCharge)
            {
                if(!playerController.BattleManager.IsHitStop)
                playerController.SpriteAnim.speed = 1;
               // playerController.SkillManager.CanCharge = false;

                //オノマトペを捕食
                playerController.AttackColliderV2.HandleSelectedClosestCollider(this.transform, 0);

                //時間の流れを元に戻す
                Time.timeScale = 1.0f;

            
            }
            //短押し
            else
            {
                //方向変更
                playerController.SetEightDirection();

                //入力方向取得
                UnityEngine.Vector2 inputDirec = inputManager.GetMoveDirec();
                //捕食方向設定
                eatingDirec = new Vector3(inputDirec.x, 0, inputDirec.y);
                if (inputDirec == Vector2.zero) eatingDirec = playerController.GetForward();

                //コライダーの検出
                playerController.AttackColliderV2.SelectColliderWithInputDirec(playerController.transform, 0.0f, eatingDirec, onomatoLayer);

                //オノマトペを捕食
                playerController.AttackColliderV2.HandleSelectedClosestCollider(this.transform, 0);

                //コライダーの検出
                // playerController.AttackColliderV2.DetectCollidersWithInputDirec(playerController.transform, 0.0f, eatingDirec, onomatoLayer);

            }

        }





        //アニメーション終了、アイドルへ遷移
        if (spriteAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            playerController.StateManager.TransitionState(StateType.Idle);
        }
    }

    public override void FixedTick()
    {

    }

    public override void Exit()
    {
    }

    public void Attack()
    {
        

        //敵との当たり判定
        playerController.AttackColliderV2.DetectColliders(1.0f, enemyLayer,false);
    }

    // Gizmosを使用してベクトルを描画
    private void OnDrawGizmos()
    {
        if (playerStateManager==null) return;
        if ( playerStateManager.CurrentStateType != StateType.Eat) return;

        // オブジェクトの位置
        Vector3 start = transform.position;
        // ベクトルの終点を計算
        Vector3 end = start + eatingDirec;

        // 線の色を設定
        Gizmos.color = Color.red;

        // 線を描画
        Gizmos.DrawLine(start, end);

        // 矢印の終点に小さな球を描画して視覚的にわかりやすくする
        Gizmos.DrawSphere(end, 0.05f);
    }
}
