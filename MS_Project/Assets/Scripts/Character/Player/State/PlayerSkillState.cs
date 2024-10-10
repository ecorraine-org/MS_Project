using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerState
{
    //攻撃test
    public UnityEngine.Vector3 attackSize = new UnityEngine.Vector3(1f, 1f, 1f);
    UnityEngine.Vector3 attackAreaPos;
    public UnityEngine.Vector3 offsetPos;
    public float attackDamage;
    public LayerMask enemyLayer;

    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(true);

        base.Init(_playerController);
        Debug.Log("スキルステート");

        //スキルを発動する
      

        switch (playerModeManager.Mode)
        {
            case PlayerMode.None:
                playerController.SkillManager.UseSkill(PlayerSkill.Sword);
                break;
            case PlayerMode.Sword:
                playerController.SkillManager.UseSkill(PlayerSkill.Sword);
                break;
            case PlayerMode.Hammer:
                playerController.SkillManager.UseSkill(PlayerSkill.Hammer);
                break;
            default:
                break;
        }
    }

    public override void Tick()
    {
        Attack();


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
        playerController.SpriteRenderer.color = Color.white;
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
