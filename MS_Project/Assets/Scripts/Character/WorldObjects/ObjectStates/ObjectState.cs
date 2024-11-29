using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectState : MonoBehaviour
{
    protected WorldObjectController objController;
    protected ObjectStateHandler objStateHandler;

    protected EnemyController enemy;
    protected EnemyStatusHandler enemyStatusHandler;
    protected EnemyAnimManager animHandler;

    protected ObjectController staticObject;
    protected ObjectStatusHandler objectStatusHandler;

    protected PlayerController player;

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
            enemyStatusHandler = enemy.EnemyStatus;
            animHandler = enemy.AnimManager;

            player = enemy.PlayerController;
        }

        else if (objController.Type == WorldObjectType.StaticObject)
        {
            staticObject = objController as ObjectController;
            objectStatusHandler = staticObject.ObjectStatus;
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
