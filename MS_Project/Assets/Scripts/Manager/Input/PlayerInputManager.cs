using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// プレイヤー操作管理シングルトン
/// </summary>
public class PlayerInputManager : SingletonBaseBehavior<PlayerInputManager>
{
    // インプットシステム
    private InputControls inputControls;


    // アクションのディクショナリ
    private Dictionary<InputAction, Action> actionMap = new Dictionary<InputAction, Action>();

    protected override void AwakeProcess()
    {
        inputControls = new InputControls();

        // 入力を有効化
        inputControls.Enable();
    }

    /// <summary>
    /// ゲーム外無効にする
    /// </summary>
    public void DisableInput()
    {
        inputControls.Disable();
    }

    /// <summary>
    /// ゲーム内プレイヤー操作を有効にする
    /// </summary>
    public void EnableInput()
    {
        inputControls.Enable();
    }

    /// <summary>
    /// 任意の入力アクションをバインドします。
    /// </summary>
    /// <param name="inputAction">バインドするインプットアクション。</param>
    /// <param name="action">実行するアクション。</param>
    public void BindAction(InputAction inputAction, Action action)
    {
        // アクションが既に存在するかチェック
        if (actionMap.ContainsKey(inputAction))
        {
            Debug.LogWarning("既にこのアクションはバインドされています。");
            return;
        }

        // ディクショナリに追加
        actionMap[inputAction] = action;
        inputAction.performed += OnActionPerformed;
    }

    /// <summary>
    /// 任意の入力アクションのバインドを解除します。
    /// </summary>
    /// <param name="inputAction">バインドを解除するインプットアクション。</param>
    public void UnBindAction(InputAction inputAction)
    {
        if (actionMap.ContainsKey(inputAction))
        {
            inputAction.performed -= OnActionPerformed;
            actionMap.Remove(inputAction);
        }
    }

    /// <summary>
    /// インプットが実行されたときに呼ばれるメソッド。
    /// </summary>
    /// <param name="ctx">入力コンテキスト。</param>
    private void OnActionPerformed(InputAction.CallbackContext ctx)
    {
        // コールバックされたアクションがあれば実行
        if (actionMap.ContainsKey(ctx.action))
        {
            actionMap[ctx.action]?.Invoke();
        }
    }


    /// <summary>
    /// 移動入力方向を取得
    /// </summary>
    public Vector2 GetMoveDirec()
    {
        return inputControls.GamePlay.Walk.ReadValue<Vector2>();
    }

    public InputControls InputControls
    {
        get { return inputControls; }
    }


}
