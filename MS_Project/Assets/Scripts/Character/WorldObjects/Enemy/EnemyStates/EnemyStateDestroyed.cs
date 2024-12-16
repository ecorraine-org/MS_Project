using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

public class EnemyStateDestroyed : EnemyState
{
    private MethodInfo diedTickMethod;
    BossDetector bossDetector;

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);
        if (enemyStatusHandler.StatusData.enemyRank == EnemyRank.Boss)
        {
            bossDetector = GameObject.Find("BossDetector").GetComponent<BossDetector>();
            bossDetector.StartCoroutine(bossDetector.SlowTimeForSeconds(1.2f));
            //É{ÉXÇè¡Ç∑
            //enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);
        }
        else
        {
            enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);
        }

        enemy.Anim.Play("Died", 0, 0.0f);

        diedTickMethod = enemy.EnemyAction.GetType().GetMethod("DiedTick");

        //?øΩG?øΩ…ÇÔøΩ?øΩ∆éÔøΩ?øΩÃèÔøΩ?øΩ?øΩ
        var method = enemy.EnemyAction.GetType().GetMethod("DiedInit");
        if (method != null)
        {
            method.Invoke(enemy.EnemyAction, null);
        }



        //?øΩ?øΩ
        //enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);

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

        //?øΩ?øΩ?øΩ÷ëJ?øΩ?øΩ
        if (enemy.AnimManager != null && enemy.AnimManager.IsAnimEnd)
        {
            //?øΩ?øΩ
            enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);
        }

        //?øΩG?øΩ…ÇÔøΩ?øΩ∆éÔøΩ?øΩÃèÔøΩ?øΩ?øΩ
        if (diedTickMethod != null)
        {
            diedTickMethod.Invoke(enemy.EnemyAction, null);
        }

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
