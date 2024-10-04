using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEatState : PlayerState
{
    //�ߐHtest
    public UnityEngine.Vector3 attackSize = new UnityEngine.Vector3(1f, 1f, 1f);
    UnityEngine.Vector3 attackAreaPos;
    public UnityEngine.Vector3 offsetPos;
    public float attackDamage;
    public LayerMask onomatoLayer;

    public override void Init(PlayerController _playerController)
    {
        base.Init(_playerController);
        Debug.Log("�ߐH�X�e�[�g");

        Attack();
        spriteAnim.Play("Attack");
    }

    public override void Tick()
    {
        //���[�h�`�F���W�֑J��
        //����:�@�J�ڐ��Ԃ͍��̂ƈႤ
        //�A����̃I�m�}�g�y��H�ׂ�
        //if ()
        //{
        //    playerController.StateManager.TransitionState(StateType.ModeChange);
        //}

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
        offsetPos.x = spriteRenderer.flipX ? Mathf.Abs(offsetPos.x) : -Mathf.Abs(offsetPos.x);

        attackAreaPos += offsetPos;

        //�R���C�_�[�̌��o
        playerController.AttackCollider.DetectColliders(attackAreaPos, attackSize, attackDamage, onomatoLayer);

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
