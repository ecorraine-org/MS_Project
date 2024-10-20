using System.Collections;
using UnityEngine;

/// <summary>
/// 向き方向
/// </summary>
public enum Direction
{
    [InspectorName("上")] Up,
    [InspectorName("下")] Down,
    [InspectorName("左")] Left,
    [InspectorName("右")] Right,
    [InspectorName("左上")] UpLeft,
    [InspectorName("右上")] UpRight,
    [InspectorName("左下")] DownLeft,
    [InspectorName("右下")] DownRight
}

/// <summary>
/// プレイヤーモード
/// </summary>
public enum PlayerMode
{
    None,
    [InspectorName("剣")] Sword,
    [InspectorName("ハンマー")] Hammer,
    [InspectorName("スピア")] Spear,
    [InspectorName("ガントレット")] Gauntlet,
}

/// <summary>
/// プレイヤースキル 
/// </summary>
/// <remarks>
/// モードと同じ順番で定義する必要がある
/// </remarks>
public enum PlayerSkill
{
    None,
    Sword,
    Hammer,
    Spear,
    Gauntlet,
    Eat,
    Dodge
}

/// <summary>
/// オノマトペ種類
/// </summary>
public enum OnomatoType
{
    None,
    [InspectorName("剣")] SlashType,
    [InspectorName("ハンマー")] SmashType,
    [InspectorName("スピアー")] PierceType,
    [InspectorName("殴り")] HandType,
    [InspectorName("その他")] OtherType
}

/// <summary>
/// 
/// </summary>
public enum WorldObjectType
{
    None,
    [InspectorName("プレイヤー")] Player,
    [InspectorName("敵")] Enemy,
    [InspectorName("その他")] StaticObject
}
