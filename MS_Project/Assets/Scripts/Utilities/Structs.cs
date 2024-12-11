using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 構造体管理
/// </summary>

[System.Serializable]//Inspectorで表示するために追加した
public struct AttackerParams
{
    public PlayerMode mode;
    public float slowSpeed;//ヒットストップによる減速
    public float stopDuration;//ヒットストップ持続時間
    public float moveSpeed;//攻撃中進む速度
}

[System.Serializable]
public struct ReceiverParams
{
    public PlayerMode mode;
    public float slowSpeed;//ヒットストップによる減速
    public float stopDuration;//ヒットストップ持続時間
    public float moveSpeed;//攻撃中進む速度
}
