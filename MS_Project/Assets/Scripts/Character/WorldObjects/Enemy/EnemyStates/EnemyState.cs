using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : WorldObjectState
{
    protected EnemyController enemy;
    protected EnemyStatusHandler enemyStatusHandler;
    protected EnemyStateHandler enemyStateHandler;
    protected EnemyAnimManager animHandler;

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        if (objController.Type != WorldObjectType.Enemy)
        {
            CustomLogger.Log("種類違いのステートが検出されました\n" + objController.name + "：" + objController.Type.ToString());
            return;
        }

        enemy = objController as EnemyController;
        if (enemy == null)
        {
            CustomLogger.Log(objController.name + "が取得できませんでした");
            return;
        }

        enemyStatusHandler = enemy.Status;
        enemyStateHandler = enemy.State;
        animHandler = enemy.AnimManager;

        player = enemy.PlayerController;
    }

    public override void Tick()
    {
        if (enemy == null)
            return;
    }

    public override void FixedTick()
    {
        if (enemy == null)
            return;
    }

    public override void Exit()
    {
        if (enemy == null)
            return;
    }
}
