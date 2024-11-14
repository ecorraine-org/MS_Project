using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinimapSettings
{
    public Vector2 MinimapSize = new Vector2(200f, 200f);   // ミニマップのサイズ
    public float MinimapZoom = 5f;                          // ミニマップのズーム
    public Vector2 MinimapOffset = new Vector2(20f, 20f);   // ミニマップのオフセット
    public Color RoomVisitedColor = Color.white;            // 部屋が訪れた時の色
    public Color RoomUnvisitedColor = Color.gray;           // 部屋が未訪問の時の色
    public Color PlayerIndicatorColor = Color.red;          // プレイヤーの位置を示す色
    public float UpdateInterval = 0.5f;                     // ミニマップの更新間隔
}