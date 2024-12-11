using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyStateDestroyed : EnemyState
{
    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);

        //if (enemyStatusHandler.StatusData.enemyRank != EnemyRank.Boss)
        //    enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);

        //else if (enemyStatusHandler.StatusData.enemyRank == EnemyRank.Boss)
        //{
        //    enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);
        //    SceneManager.LoadScene("Result");
        //}
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
