using System.Collections;
using UnityEngine;

/// <summary>
/// 向き方向
/// </summary>
public enum Direction
{
    [InspectorName("上")]
    Up,
    [InspectorName("下")]
    Down,
    [InspectorName("左")]
    Left,
    [InspectorName("右")]
    Right,
    [InspectorName("左上")]
    UpLeft,
    [InspectorName("右上")]
    UpRight,
    [InspectorName("左下")]
    DownLeft,
    [InspectorName("右下")]
    DownRight
}

/// <summary>
/// プレイヤーモード
/// </summary>
public enum PlayerMode
{
    None,
    [InspectorName("剣")]
    Sword,
    [InspectorName("ハンマー")]
    Hammer,
}

/// <summary>
/// オノマトペ種類
/// </summary>
public enum OnomatoType
{
    None,
    [InspectorName("剣")]
    A,
    [InspectorName("ハンマー")]
    B,
    [InspectorName("C")]
    C,
    [InspectorName("D")]
    D
}
