using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステートの親クラス
/// </summary>
public abstract class PlayerState : MonoBehaviour
{
    // インプットマネージャーシングルトン
    protected PlayerInputManager inputManager;

    protected PlayerController playerController;
    protected PlayerStateManager playerStateManager;
    // protected PlayerStatusManager playerStatusManager;

    protected Rigidbody rb;

    protected Animator spriteAnim;

    protected Transform mainCamera;

    /// <summary>
    ///ステートの初期化処理
    /// </summary>
    public virtual void Init(PlayerController _playerController)
    {
        inputManager = PlayerInputManager.Instance;

        //マネージャー取得
        playerController = _playerController;

        playerStateManager = playerController.StateManager;
        //playerStatusManager = playerController.EnemyStatusManager;

        rb = playerController.RigidBody;

        spriteAnim = playerController.SpriteAnim;

        mainCamera = playerController.MainCamera;
    }

    /// <summary>
    ///ステートの更新処理
    /// </summary>
    public abstract void Tick();

    /// <summary>
    ///ステートの更新処理
    /// </summary>
    public abstract void FixedTick();

    /// <summary>
    ///ステートの終了処理
    /// </summary>
    public abstract void Exit();
}
