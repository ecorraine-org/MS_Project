using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    //�U��test
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
        //�_���[�W�`�F�b�N
        playerController.StateManager.CheckHit();

        // Debug.Log("Time" + spriteAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);

        //�A�j���[�V�����I���A�A�C�h���֑J��
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

        //���E���]��
        offsetPos.x = spriteRenderer.flipX ? -Mathf.Abs(offsetPos.x) : Mathf.Abs(offsetPos.x);

        attackAreaPos += offsetPos;

        Debug.Log("controller Attack");
        Collider[] hitColliders = Physics.OverlapBox(attackAreaPos, attackSize / 2, UnityEngine.Quaternion.identity, enemyLayer);


        if (hitColliders.Length <= 0) return;

        Debug.Log("�����蔻��" + hitColliders[0]);//test

        foreach (Collider hitCollider in hitColliders)
        {
            hitCollider.GetComponentInChildren<ILife>().TakeDamage(attackDamage);
        }
    }

    /// <summary>
    /// �`��test
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackAreaPos, attackSize);
    }
}
