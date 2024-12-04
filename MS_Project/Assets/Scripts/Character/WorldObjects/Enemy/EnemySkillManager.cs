using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵スキル管理
/// </summary>
public class EnemySkillManager : MonoBehaviour
{
    [SerializeField, Header("突進処理ビヘイビア")]
    DashHandler dash;

    // EnemyControllerの参照
    EnemyController enemyController;

    public void Init(EnemyController _enemy)
    {
        enemyController = _enemy;

        dash.Init(enemyController);
    }

    // キャンセルされた時のリセット処理
    public void Reset()
    {
        if (dash.IsDashing) dash.End();
    }

    public DashHandler DashHandler
    {
        get => this.dash;
    }

    public bool IsDashing
    {
        get => this.dash.IsDashing;
        set { this.dash.IsDashing = value; }
    }

    public Vector3 DashDirec
    {
        get => this.dash.Direc;
    }
}
