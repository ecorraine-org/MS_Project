using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyStateDestroyed : EnemyState
{
    //ŽžŠÔŒv‘ª
    float frameTime = 0.0f;
    protected Animator animator;


    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        enemy.Anim.Play("Damaged", 0, 0.0f);


        //enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);

        //if (enemyStatusHandler.StatusData.enemyRank != EnemyRank.Boss)
        //    enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);

        //else if (enemyStatusHandler.StatusData.enemyRank == EnemyRank.Boss)
        //{
        //    enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);
        //    SceneManager.LoadScene("Result");
        //}
    }

    public void Update()
    {
    }

    public override void Tick()
    {
        base.Tick();

        //Ž€‚Ö‘JˆÚ
        if (enemy.AnimManager != null && enemy.AnimManager.IsAnimEnd)
        {
            //Ž€
            enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);
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
