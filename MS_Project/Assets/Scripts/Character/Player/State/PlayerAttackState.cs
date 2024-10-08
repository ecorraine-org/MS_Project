using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    //攻撃test
    public UnityEngine.Vector3 attackSize = new UnityEngine.Vector3(1f, 1f, 1f);
    UnityEngine.Vector3 attackAreaPos;
    public UnityEngine.Vector3 offsetPos;
    public float attackDamage;
    public LayerMask enemyLayer;

    //仮処理
    //private float testTimer;

    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(true);

        base.Init(_playerController);

        switch (playerModeManager.Mode)
        {
            case PlayerMode.None:
                spriteAnim.Play("Attack");
                break;
            case PlayerMode.Sword:
                spriteAnim.Play("Attack");
                break;
            case PlayerMode.Hammer:
                spriteAnim.Play("HammerAttack");
                break;
            default:
                break;
        }

        //仮のクールタイマー初期化
        // testTimer = 0;
    }

    public override void Tick()
    {
        //ダメージチェック
        playerController.StateManager.CheckHit();

        //仮のクールタイム設定
        // testTimer += Time.time;
        // if (testTimer >= spriteAnim.GetCurrentAnimatorStateInfo(0).length + 0.2f)
        //  {
        //     testTimer = 0;
        Attack();
        // }


        // Debug.Log("Time" + spriteAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);

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
        playerController.AttackCollider.DetectColliders(attackAreaPos, attackSize, attackDamage, enemyLayer);

    }

    /// <summary>
    /// 描画test
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackAreaPos, attackSize);
    }

}



