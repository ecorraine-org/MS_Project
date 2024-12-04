using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDamaged : EnemyState
{
    public override void Init(WorldObjectController _objectController)
    {
        base.Init(_objectController);

        enemy.Anim.Play("Damaged", 0, 0.0f);

        int playerMode = ((int)player.ModeManager.Mode);
        OnomatopoeiaData attackOnomatoData = player.StatusManager.StatusData.onomatoAttackData[playerMode];
        enemy.GenerateOnomatopoeia(enemy.gameObject, attackOnomatoData);

        enemy.DashHandler.Speed = enemyStatusHandler.StatusData.knockBackSpeed;
        enemy.DashHandler.Duration = enemyStatusHandler.StatusData.knockBackDuration;
        Vector3 playerDirec = player.transform.position - transform.position;
        playerDirec.y = 0;
        playerDirec.z = 0;
        enemy.DashHandler.Begin(false, -1 * playerDirec.normalized);
    }

    public override void Tick()
    {
        base.Tick();

        //死亡チェック
        if (enemyStateHandler.CheckDeath()) return;

        //ダメージ(連撃)チェック
        if (enemyStateHandler.CheckHit()) return;

        //アイドルへ遷移
        if (enemy.AnimManager != null && enemy.AnimManager.IsAnimEnd)
        {
            //Debug.Log("ダメージ");
            enemyStateHandler.TransitionState(ObjectStateType.Idle);
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
