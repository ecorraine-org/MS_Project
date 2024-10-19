using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinishState : PlayerState
{
    //�ߐHtest
    public UnityEngine.Vector3 attackSize = new UnityEngine.Vector3(1f, 1f, 1f);
    UnityEngine.Vector3 attackAreaPos;
    public UnityEngine.Vector3 offsetPos;
    public float attackDamage;

    [SerializeField, Header("�G���C���[")]
    private LayerMask enemyLayer;

    //�ߐH����
    Vector3 eatingDirec;

    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(true);

        base.Init(_playerController);
        Debug.Log("�I���X�e�[�g");

        //���͕����擾
        UnityEngine.Vector2 inputDirec = inputManager.GetMoveDirec();

        //�ߐH�����ݒ�
        eatingDirec = new Vector3(inputDirec.x, 0, inputDirec.y);

        //����
        playerController.SkillManager.ExecuteEat();
    }

    public override void Tick()
    {
        Attack();

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
        //playerController.AttackCollider.DetectCollidersWithInputDirec(playerController.transform, attackAreaPos, attackSize, 0.0f, eatingDirec, onomatoLayer);

        //�G�Ƃ̓����蔻��
        playerController.AttackCollider.DetectColliders(attackAreaPos, attackSize, 1.0f, enemyLayer,true);
    }

    /// <summary>
    /// �`��test
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackAreaPos, attackSize);
    }

    // Gizmos���g�p���ăx�N�g����`��
    private void OnDrawGizmos()
    {
        if (playerStateManager == null) return;
        if (playerStateManager.CurrentStateType != StateType.Eat) return;

        // �I�u�W�F�N�g�̈ʒu
        Vector3 start = transform.position;
        // �x�N�g���̏I�_���v�Z
        Vector3 end = start + eatingDirec;

        // ���̐F��ݒ�
        Gizmos.color = Color.red;

        // ����`��
        Gizmos.DrawLine(start, end);

        // ���̏I�_�ɏ����ȋ���`�悵�Ď��o�I�ɂ킩��₷������
        Gizmos.DrawSphere(end, 0.05f);
    }
}
