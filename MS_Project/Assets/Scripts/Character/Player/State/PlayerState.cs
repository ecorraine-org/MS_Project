using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̃X�e�[�g�̐e�N���X
/// </summary>
public abstract class PlayerState : MonoBehaviour
{
    // �C���v�b�g�}�l�[�W���[�V���O���g��
    protected PlayerInputManager inputManager;

    protected PlayerController playerController;
    protected PlayerStateManager playerStateManager;
    // protected PlayerStatusManager playerStatusManager;

    protected Rigidbody rb;

    protected Animator spriteAnim;

    protected Transform mainCamera;

    /// <summary>
    ///�X�e�[�g�̏���������
    /// </summary>
    public virtual void Init(PlayerController _playerController)
    {
        inputManager = PlayerInputManager.Instance;

        //�}�l�[�W���[�擾
        playerController = _playerController;

        playerStateManager = playerController.StateManager;
        //playerStatusManager = playerController.EnemyStatusManager;

        rb = playerController.RigidBody;

        spriteAnim = playerController.SpriteAnim;

        mainCamera = playerController.MainCamera;
    }

    /// <summary>
    ///�X�e�[�g�̍X�V����
    /// </summary>
    public abstract void Tick();

    /// <summary>
    ///�X�e�[�g�̍X�V����
    /// </summary>
    public abstract void FixedTick();

    /// <summary>
    ///�X�e�[�g�̏I������
    /// </summary>
    public abstract void Exit();
}
