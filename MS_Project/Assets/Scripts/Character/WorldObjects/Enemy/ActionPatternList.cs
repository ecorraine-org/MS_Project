using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ボス攻撃パターンビヘイビア
/// </summary>
[System.Serializable]
public struct ActionPattern
{
    // public float prepareTime;    //準備時間
    public UnityEvent OnAction; //行動イベント
    public float recoveryTime;  //アイドルに戻ってからカウントするクールタイム
  
}

public class ActionPatternList : MonoBehaviour
{
    [SerializeField, Header("行動リスト")]
    ActionPattern[] actionPattern;

    public ActionPattern[] GetActionPattern()
    {
        return actionPattern;
    }
}
