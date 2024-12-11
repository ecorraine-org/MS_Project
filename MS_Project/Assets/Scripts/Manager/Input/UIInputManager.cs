using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// UI操作管理シングルトン
/// </summary>
public class UIInputManager : SingletonBaseBehavior<UIInputManager>
{
    // インプットシステム
    private InputControls inputControls;

  //  private PlayerController player;

    //Lスティック方向
    Vector3 lStickVec3;

    // アクションのディクショナリ
    private Dictionary<InputAction, Action> actionMap = new Dictionary<InputAction, Action>();

    protected override void AwakeProcess()
    {
        inputControls = new InputControls();

        // 入力を有効化
       // inputControls.Enable();
    }

    private void Update()
    {
       // GetMoveDirec();
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
    /// 移動入力方向を取得
    /// </summary>
    public Vector2 GetMoveDirec()
    {
    
        if (inputControls.UI.Move.triggered)
        {
            return inputControls.UI.Move.ReadValue<Vector2>();
        }

        return Vector2.zero;
    }

    /// <summary>
    /// 確認入力(トリガー)を取得
    /// </summary>
    public bool GetEnterTrigger()
    {
        return inputControls.UI.Enter.triggered;
    }

    /// <summary>
    /// 確認入力(ボタン長押し)を取得
    /// </summary>
    public bool GetEnterPressed()
    {
        return inputControls.UI.Enter.IsPressed();
    }

    /// <summary>
    /// 確認入力(ボタン離す)を取得
    /// </summary>
    public bool GetEnterReleased()
    {
        return inputControls.UI.Enter.WasReleasedThisFrame();
    }

    /// <summary>
    /// キャンセル入力を取得
    /// </summary>
    public bool GetCancelTrigger()
    {
        return inputControls.UI.Cancel.triggered;
    }

    /// <summary>
    /// 任意入力を取得
    /// </summary>
    public bool GetAnyKeyTrigger()
    {
        return inputControls.UI.AnyKey.triggered;
    }

    /// <summary>
    /// 移動入力(トリガー)を取得
    /// </summary>
    public bool GetUPTrigger()
    {
        return inputControls.UI.UP.triggered;
    }

    public bool GetDownTrigger()
    {
        return inputControls.UI.DOWN.triggered;
    }

    public bool GetRightTrigger()
    {
        return inputControls.UI.RIGHT.triggered;
    }

    public bool GetLeftTrigger()
    {
        return inputControls.UI.LEFT.triggered;
    }

    public bool GetStartTrigger()
    {
        return inputControls.UI.START.triggered;
    }



    public InputControls InputControls
    {
        get { return inputControls; }
    }

 

}




/// <summary>
/// 任意の入力アクションをバインドします。
/// </summary>
/// <param name="inputAction">バインドするインプットアクション。</param>
/// <param name="action">実行するアクション。</param>
//public void BindAction(InputAction inputAction, Action action)
//{
//    // アクションが既に存在するかチェック
//    if (actionMap.ContainsKey(inputAction))
//    {
//        Debug.LogWarning("既にこのアクションはバインドされています。");
//        return;
//    }

//    // ディクショナリに追加
//    actionMap[inputAction] = action;
//    inputAction.performed += OnActionPerformed;
//}

///// <summary>
///// 任意の入力アクションのバインドを解除します。
///// </summary>
///// <param name="inputAction">バインドを解除するインプットアクション。</param>
//public void UnBindAction(InputAction inputAction)
//{
//    if (actionMap.ContainsKey(inputAction))
//    {
//        inputAction.performed -= OnActionPerformed;
//        actionMap.Remove(inputAction);
//    }
//}

///// <summary>
///// インプットが実行されたときに呼ばれるメソッド。
///// </summary>
///// <param name="ctx">入力コンテキスト。</param>
//private void OnActionPerformed(InputAction.CallbackContext ctx)
//{
//    // コールバックされたアクションがあれば実行
//    if (actionMap.ContainsKey(ctx.action))
//    {
//        actionMap[ctx.action]?.Invoke();
//    }
//}
