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

    protected PlayerStatusManager statusManager;

    protected PlayerSkillManager playerSkillManager;

    protected PlayerModeManager playerModeManager;

    protected PlayerAnimManager animManager;

    protected Rigidbody rb;

    protected Animator spriteAnim;

    protected SpriteRenderer spriteRenderer;

    private bool isPerformDamage = false;

    /// <summary>
    ///ステートの初期化処理
    /// </summary>
    public virtual void Init(PlayerController _playerController)
    {
        inputManager = PlayerInputManager.Instance;

        //マネージャー取得
        playerController = _playerController;

        playerStateManager = playerController.StateManager;

        statusManager = playerController.StatusManager;

        playerSkillManager = playerController.SkillManager;

        playerModeManager = playerController.ModeManager;

        animManager = playerController.AnimManager;

        rb = playerController.RigidBody;

        spriteAnim = playerController.SpriteAnim;

        spriteRenderer = playerController.SpriteRenderer;

        DrawAsOverlay(isPerformDamage);
    }

    public void DrawAsOverlay(bool _value)
    {
        if (!_value)
        {
            // 深度テストを有効
            playerController.material.SetFloat("_ZTest", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
        }
        else
        {
            // 深度テストを無効
            playerController.material.SetFloat("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        }
    }

    public void SetIsPerformDamage(bool _value)
    {
        isPerformDamage = _value;
    }

    public bool GetIsPerformDamage()
    {
        return isPerformDamage;
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
