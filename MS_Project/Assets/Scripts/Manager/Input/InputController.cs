using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UIとプレイヤー操作の切り替えを制御するシングルトン
/// </summary>
public class InputController : SingletonBaseBehavior<InputController>
{
    public enum InputContext
    {
        Player,
        UI
    }

    [SerializeField,NonEditable,Header("入力モード(UI or Player)")]
    InputContext currentContext/*= InputContext.UI*/;

    //private void OnValidate()
    //{
    //    ApplyInputContextChange();
    //}

    ///// <summary>
    ///// inspectorの変更適用
    ///// </summary>
    //private void ApplyInputContextChange()
    //{
    //    if (currentContext == InputContext.UI)
    //    {
    //        Debug.Log("UI モードに切り替えました");
    //        SetInputContext(InputContext.UI);
    //    }
    //    else if (currentContext == InputContext.Player)
    //    {
    //        Debug.Log("プレイヤーモードに切り替えました");
    //        SetInputContext(InputContext.Player);
    //    }
    //}

    protected override void AwakeProcess()
    {
        //シーン始める時も実行する(存在している場合)
        SetInputContext(InputContext.UI);
    }

    /// <summary>
    /// 操作の切り替え
    /// </summary>
    /// <param name="context"></param>
    /// <note>
    /// 使用例
    /// InputController.Instance.SetInputContext(InputController.InputContext.UI);
    /// </note>
    public void SetInputContext(InputContext context)
    {
        currentContext = context;

        switch (currentContext)
        {
            case InputContext.Player:
                PlayerInputManager.Instance.EnableInput();
                UIInputManager.Instance.DisableInput();
                break;

            case InputContext.UI:
                PlayerInputManager.Instance.DisableInput();
                UIInputManager.Instance.EnableInput();
                break;
        }
    }
}
