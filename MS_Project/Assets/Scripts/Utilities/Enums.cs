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
    [InspectorName("メリケンサック")] Gauntlet,
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
    [InspectorName("剣スキル")] Sword,
    [InspectorName("ハンマースキル")] Hammer,
    [InspectorName("スピアスキル")] Spear,
    [InspectorName("メリケンサックスキル")] Gauntlet,
    [InspectorName("捕食")] Eat,
    [InspectorName("回避")] Dodge
}

/// <summary>
/// オノマトペ種類
/// </summary>
public enum OnomatoType
{
    None,
    [InspectorName("斬撃系")] SlashType,
    [InspectorName("打撃系")] SmashType,
    [InspectorName("突撃系")] PierceType,
    [InspectorName("殴打系")] PunchType,
    [InspectorName("その他")] OtherType
}

/// <summary>
/// オブジェクト種類
/// </summary>
public enum WorldObjectType
{
    None,
    [InspectorName("プレイヤー")] Player,
    [InspectorName("敵")] Enemy,
    [InspectorName("その他")] StaticObject
}

/// <summary>
/// エネミー階級
/// </summary>
public enum EnemyRank
{
    None,
    [InspectorName("雑魚")] Normal,
    [InspectorName("エリート")] Elite,
    [InspectorName("ボス")] Boss,
}
