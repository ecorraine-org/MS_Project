using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

public class EnemyStateDestroyed : EnemyState
{
    private MethodInfo diedTickMethod;


    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        enemy.Anim.Play("Died", 0, 0.0f);

        diedTickMethod = enemy.EnemyAction.GetType().GetMethod("DiedTick");

        //ìGÇ…ÇÊÇÈì∆é©ÇÃèàóù
        var method = enemy.EnemyAction.GetType().GetMethod("DiedInit");
        if (method != null)
        {
            method.Invoke(enemy.EnemyAction, null);
        }


        //éÄ
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

        //éÄÇ÷ëJà⁄
        if (enemy.AnimManager != null && enemy.AnimManager.IsAnimEnd)
        {
            //éÄ
            enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);
        }

        //ìGÇ…ÇÊÇÈì∆é©ÇÃèàóù
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
