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
    [InspectorName("プレイヤー"), Tooltip("プレイヤー")] Player,
    [InspectorName("敵"), Tooltip("敵")] Enemy,
    [InspectorName("固定オブジェクト"), Tooltip("固定オブジェクト")] StaticObject
}

/// <summary>
/// エネミー階級
/// </summary>
public enum EnemyRank
{
    None,
    [InspectorName("雑魚"), Tooltip("雑魚")] Normal,
    [InspectorName("エリート"), Tooltip("エリート")] Elite,
    [InspectorName("ボス"), Tooltip("ボス")] Boss,
}

/// <summary>
/// カメラ種類
/// </summary>
public enum CameraType
{
    [InspectorName("メインカメラ")] Main,
    [InspectorName("UIカメラ")] UI,
    [InspectorName("ミニマップカメラ")] MiniMap,
    [InspectorName("カットシーンカメラ")] CutScene
}

/// <summary>
/// カメラエフェクト種類
/// </summary>
public enum CameraEffectType
{
    None,
    [InspectorName("フェード")] Fade,
    [InspectorName("シェイク")] Shake,
    [InspectorName("ズーム")] Zoom,
    [InspectorName("フリーズ")] Freeze,
    [InspectorName("カット")] Cut,
    [InspectorName("フィルター")] Filter,
    [InspectorName("ロール")] Roll,
    [InspectorName("パン")] Pan,
}

/// <summary>
/// ミッションタイプ
/// </summary>
public enum MissionType
{
    None,
    [InspectorName("敵殲滅"), Tooltip("敵殲滅")] KillAll,
    [InspectorName("ボス撃破"), Tooltip("ボス撃破")] KillBoss,
    [InspectorName("道開き"), Tooltip("道開き")] OpenRoute,
    [InspectorName("保護"), Tooltip("保護")] Protect
}

/// <summary>
/// プレイヤースキル
/// </summary>
/// <remarks>
/// モードと同じ順番で定義する必要がある
/// </remarks>
public enum PlayerEffect
{
    None,
    [InspectorName("剣攻撃1")] SwordAttack1,
    [InspectorName("剣攻撃2")] SwordAttack2,
    [InspectorName("スピア攻撃")] SpearAttack,
    [InspectorName("ハンマー攻撃")] HammerAttack,
}

/// <summary>
/// チュートリアル段階
/// </summary>
public enum TutorialStage
{
    None,
    Step1,
    Step2,
    Step3,
    Step4,
    Step5
}
