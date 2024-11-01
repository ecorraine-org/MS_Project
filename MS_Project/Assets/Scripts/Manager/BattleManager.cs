using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトル関連
/// </summary>
public class BattleManager : SingletonBaseBehavior<BattleManager>
{
    [SerializeField, Header("ヒットリアクションデータ")]
    PlayerHitData playerHitData;

    [SerializeField, Header("プレイヤーモード")]
    PlayerMode curPlayerMode;

    protected override void AwakeProcess()
    {
        if (playerHitData == null)
        {
            Debug.LogError("playerHitDataが設定されていません", this);
        }

    }

    public HitReaction GetPlayerHitReaction()
    {
        return playerHitData.dicHitReac[curPlayerMode];
    }
    

    public PlayerHitData PlayerHitData
    {
        get => this.playerHitData;
      //  set { this.lStickVec3 = value; }
    }

    public PlayerMode CurPlayerMode
    {
        get => this.curPlayerMode;
          set { this.curPlayerMode = value; }
    }
}
