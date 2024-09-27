using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のステータスを管理するビヘイビア
/// </summary>
public class ProtoEnemyStatusManager : StatusManager
{

    protected override void Awake()
    {
        base.Awake();

    }

    public new EnemyStatusData StatusData
    {
        get => (EnemyStatusData)statusData;
    }
}
