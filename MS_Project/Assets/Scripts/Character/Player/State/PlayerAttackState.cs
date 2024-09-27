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

    public override void Init(PlayerController _playerController)
    {
        base.Init(_playerController);
        Debug.Log("AttackState");//test

        Attack();
        spriteAnim.Play("Attack");
    }

    public override void Tick()
    {
        //ダメージチェック
        playerController.StateManager.CheckHit();

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
        offsetPos.x = spriteRenderer.flipX ? -Mathf.Abs(offsetPos.x) : Mathf.Abs(offsetPos.x);

        attackAreaPos += offsetPos;

        Debug.Log("controller Attack");
        Collider[] hitColliders = Physics.OverlapBox(attackAreaPos, attackSize / 2, UnityEngine.Quaternion.identity, enemyLayer);


        if (hitColliders.Length <= 0) return;

        Debug.Log("当たり判定" + hitColliders[0]);//test

        foreach (Collider hitCollider in hitColliders)
        {
            hitCollider.GetComponentInChildren<ILife>().TakeDamage(attackDamage);
        }
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
