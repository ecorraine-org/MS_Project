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

    private PlayerController player;

    //Lスティック方向
    Vector3 lStickVec3;

    // アクションのディクショナリ
    private Dictionary<InputAction, Action> actionMap = new Dictionary<InputAction, Action>();

    protected override void AwakeProcess()
    {
        inputControls = new InputControls();

        // 入力を有効化
        inputControls.Enable();
    }

    public void Init(PlayerController _playerController)
    {
        player = _playerController;
    }

    private void Update()
    {
        GetLStick();
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

    /// <summary>
    /// スティックの入力方向を取得
    /// </summary>
    public Vector3 GetLStick()
    {
        Vector2 inputDirec2 = inputControls.GamePlay.Walk.ReadValue<Vector2>();

        if (inputDirec2==Vector2.zero) return Vector3.zero;

         lStickVec3 = new Vector3(inputDirec2.x, 0, inputDirec2.y);

        return lStickVec3;
    }

    public Vector3 LStickVec3
    {
        get => this.lStickVec3;
        set { this.lStickVec3 = value; }
    }

    /// <summary>
    /// 攻撃入力(トリガー)を取得
    /// </summary>
    public bool GetAttackTrigger()
    {
        return inputControls.GamePlay.Attack.triggered;
    }

    /// <summary>
    /// 捕食入力を取得
    /// </summary>
    public bool GetEatTrigger()
    {
        return inputControls.GamePlay.Eat.triggered;
    }

    /// <summary>
    /// スキル入力を取得
    /// </summary>
    public bool GetSkillTrigger()
    {
        return inputControls.GamePlay.Skill.triggered;
    }

    /// <summary>
    /// ダッシュ入力を取得
    /// </summary>
    public bool GetDashTrigger()
    {
        return inputControls.GamePlay.Dash.triggered;
    }


    public InputControls InputControls
    {
        get { return inputControls; }
    }

    private void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(player.transform.position, player.transform.position + lStickVec3);
    }

}
