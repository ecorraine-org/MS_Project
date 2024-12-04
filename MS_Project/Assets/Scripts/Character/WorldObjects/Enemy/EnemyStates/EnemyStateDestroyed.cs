using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyStateDestroyed : EnemyState
{
    private EnemySpawner spawnPool;

    private void Start()
    {
        spawnPool = GameObject.FindGameObjectWithTag("Spawner").gameObject.GetComponent<EnemySpawner>();
    }

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        if (enemyStatusHandler.StatusData.enemyRank != EnemyRank.Boss)
            spawnPool.DespawnEnemyFromPool(enemy.gameObject);

        else if (enemyStatusHandler.StatusData.enemyRank == EnemyRank.Boss)
        {
            spawnPool.DespawnEnemyFromPool(enemy.gameObject);
            SceneManager.LoadScene("Result");
        }
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
