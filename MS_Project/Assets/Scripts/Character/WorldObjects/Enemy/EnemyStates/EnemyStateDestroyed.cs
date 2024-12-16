using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using PixelCrushers.SceneStreamer;

public class EnemyStateDestroyed : EnemyState
{
    private MethodInfo diedTickMethod;
    private BossDetector bossDetector;
    private bool isProcessingDeath = false;
    private float deathAnimationTime = 3.0f; // ボス死亡アニメーションの想定時間

    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        if (enemyStatusHandler.StatusData.enemyRank == EnemyRank.Boss)
        {
            HandleBossDeath();
        }
        else
        {
            HandleNormalEnemyDeath();
        }

        // 共通の初期化処理
        enemy.Anim.Play("Died", 0, 0.0f);
        diedTickMethod = enemy.EnemyAction.GetType().GetMethod("DiedTick");

        // カスタム死亡初期化メソッドの呼び出し
        var method = enemy.EnemyAction.GetType().GetMethod("DiedInit");
        if (method != null)
        {
            method.Invoke(enemy.EnemyAction, null);
        }
    }

    private void HandleBossDeath()
    {
        if (isProcessingDeath) return;
        isProcessingDeath = true;

        bossDetector = GameObject.Find("BossDetector")?.GetComponent<BossDetector>();
        if (bossDetector != null)
        {
            bossDetector.StartCoroutine(ProcessBossDeath());
        }
        else
        {
            Debug.LogError("BossDetector not found!");
        }
    }

    private void HandleNormalEnemyDeath()
    {
        enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);
    }

    private IEnumerator ProcessBossDeath()
    {
        // スロー演出開始
        if (bossDetector != null)
        {
            bossDetector.StartCoroutine(bossDetector.SlowTimeForSeconds(1.2f));
        }

        // 死亡アニメーション完了待機
        float elapsedTime = 0f;
        while (elapsedTime < deathAnimationTime)
        {
            if (enemy.AnimManager != null && enemy.AnimManager.IsAnimEnd)
            {
                break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ボスをプールから削除
        enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);

        // リザルトシーン遷移のトリガー
        if (enemy.EnemySpawner != null)
        {
            enemy.EnemySpawner.HandleBossDefeated();
        }
    }

    public override void Tick()
    {
        base.Tick();

        if (!isProcessingDeath && enemy.AnimManager != null && enemy.AnimManager.IsAnimEnd)
        {
            if (enemyStatusHandler.StatusData.enemyRank != EnemyRank.Boss)
            {
                enemy.EnemySpawner.DespawnEnemyFromPool(enemy.gameObject);
            }
        }

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