using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEatState : PlayerState
{
    //捕食test
    public UnityEngine.Vector3 attackSize = new UnityEngine.Vector3(1f, 1f, 1f);
    UnityEngine.Vector3 attackAreaPos;
    public UnityEngine.Vector3 offsetPos;
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
        Debug.Log("捕食ステート");

        //方向変更
        playerController.SetEightDirection();

        //入力方向取得
        UnityEngine.Vector2 inputDirec = inputManager.GetMoveDirec();

        //捕食方向設定
        eatingDirec = new Vector3(inputDirec.x, 0, inputDirec.y);

        //暴走時、クールタイムを無視する
        if (playerController.StatusManager.IsFrenzy)
        {
            playerController.SkillManager.ExecuteSkill(PlayerSkill.Eat);
        }
        else
        {
            playerController.SkillManager.UseSkill(PlayerSkill.Eat);
        }
       
    }

    public override void Tick()
    {
        Attack();

        //モードチェンジへ遷移
        //条件:①遷移先状態は今のと違う
        //②特定のオノマトペを食べる
        //if ()
        //{
        //    playerController.StateManager.TransitionState(StateType.ModeChange);
        //}

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

        attackAreaPos = transform.position;

        //左右反転か
        offsetPos.x = spriteRenderer.flipX ? Mathf.Abs(offsetPos.x) : -Mathf.Abs(offsetPos.x);

        attackAreaPos += offsetPos;

        //コライダーの検出
        playerController.AttackCollider.DetectCollidersWithInputDirec(playerController.transform, attackAreaPos, attackSize, 0.0f, eatingDirec, onomatoLayer);

        //敵との当たり判定
        playerController.AttackCollider.DetectColliders(attackAreaPos, attackSize, 1.0f, enemyLayer,false);
    }

    /// <summary>
    /// 描画test
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackAreaPos, attackSize);
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
