using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectState : MonoBehaviour
{
    protected ObjectController objController;

    protected ObjectStatusHandler objStatusHandler;
    protected ObjectStateHandler objStateHandler;

    protected EnemyController enemy;
    protected EnemyAnimManager animHandler;

    /// <summary>
    /// ステートの初期化処理
    /// </summary>
    public virtual void Init(ObjectController _objectController)
    {
        if (objController != null) return;

        //マネージャー取得
        objController = _objectController;
        objStatusHandler = objController.Status;
        objStateHandler = objController.State;

        if (objStatusHandler.StatusData.objectType == WorldObjectType.Enemy)
        {
            enemy = objController as EnemyController;
            animHandler = enemy.AnimManager;
        }

        objStateHandler.isAttacking = false;
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
