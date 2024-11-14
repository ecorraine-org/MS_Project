using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinimapRoom
{
    public string RoomId;   // ルームID
    public Bounds RoomBounds;   // ルームのバウンディングボックス
    public bool IsVisited;      // ルームが訪れたかどうか
    public bool IsActive;       // ルームがアクティブかどうか
    public List<string> ConnectedRooms = new(); // 接続されたルームのID
}