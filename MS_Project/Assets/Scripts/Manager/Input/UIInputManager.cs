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
    /// 捕食入力(ボタン長押し)を取得
    /// </summary>
    //public bool GetEatPressed()
    //{
    //    return inputControls.GamePlay.Eat.IsPressed();
    //}

    /// <summary>
    /// 捕食入力(ボタン離す)を取得
    /// </summary>
    //public bool GetEatReleased()
    //{
    //    return inputControls.GamePlay.Eat.WasReleasedThisFrame();
    //}

    /// <summary>
    /// 捕食入力を取得
    /// </summary>
    //public bool GetSkillTrigger()
    //{
    //    return inputControls.GamePlay.Skill.triggered;
    //}

    /// <summary>
    /// スキル入力(ボタン長押し)を取得
    /// </summary>
    //public bool GetSkillPressed()
    //{
    //    return inputControls.GamePlay.Skill.IsPressed();
    //}

    /// <summary>
    /// スキル入力(ボタン離す)を取得
    /// </summary>
    //public bool GetSkillReleased()
    //{
    //    return inputControls.GamePlay.Skill.WasReleasedThisFrame();
    //}

    /// <summary>
    /// ダッシュ入力を取得
    /// </summary>
    //public bool GetDashTrigger()
    //{
    //    return inputControls.GamePlay.Dash.triggered;
    //}


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
