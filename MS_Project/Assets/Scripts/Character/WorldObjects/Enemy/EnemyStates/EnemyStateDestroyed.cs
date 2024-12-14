using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyStateDestroyed : EnemyState
{
    BossDetector bossDetector;
    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);
        //ボスの場合は時間を遅らせるコルーチンを実行
        if (enemyStatusHandler.StatusData.enemyRank == EnemyRank.Boss)
        {
            bossDetector = GameObject.Find("BossDetector").GetComponent<BossDetector>();
            bossDetector.StartCoroutine(bossDetector.SlowTimeForBossDeath());
            //コルーチンを実行した後にボスを消す
            enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);
        }
        else
        {
            enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);
        }
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
