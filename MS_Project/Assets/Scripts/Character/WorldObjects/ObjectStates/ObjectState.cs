using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectState : MonoBehaviour
{
    protected WorldObjectController objController;
    protected ObjectStateHandler objStateHandler;

    protected EnemyStatusHandler objStatusHandler;

    protected EnemyController enemy;
    protected EnemyAnimManager animHandler;

    /// <summary>
    /// ステートの初期化処理
    /// </summary>
    public virtual void Init(WorldObjectController _objectController)
    {
        if (objController != null) return;

        //マネージャー取得
        objController = _objectController;
        objStateHandler = objController.State;

        if (objController.Type == WorldObjectType.Enemy)
        {
            enemy = objController as EnemyController;
            objStatusHandler = enemy.EnemyStatus;
            animHandler = enemy.AnimManager;
        }
    }

    /// <summary>
    /// ステートの更新処理
    /// </summary>
    public abstract void Tick();

    /// <summary>
    /// ステートの更新処理
    /// </summary>
    public abstract void FixedTick();

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    public abstract void Exit();

}
