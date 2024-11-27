using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateDestroyed : ObjectState
{
    private Collector spawnPool;

    private void Start()
    {
        spawnPool = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject.GetComponent<Collector>();
    }

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        if (enemy != null)
        {
            if (enemy.EnemyStatus.StatusData.enemyRank != EnemyRank.Boss)
                spawnPool.DespawnEnemyFromPool(enemy.gameObject);

            else if (enemy.EnemyStatus.StatusData.enemyRank == EnemyRank.Boss)
            {
                spawnPool.DespawnEnemyFromPool(enemy.gameObject);
                SceneManager.LoadScene("Result");
            }
        }
    }

    public override void Tick()
    {
    }

    public override void FixedTick()
    {
    }

    public override void Exit()
    {
    }
}
