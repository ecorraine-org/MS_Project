using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のアニメーションイベント管理
/// </summary>
public class EnemyAnimManager : AnimManager
{
    EnemyController enemy;
    [SerializeField,NonEditable, Header("アタックコライダー")]
    AttackColliderManagerV2 attackColliderV2 ;

    public void Init(EnemyController _enemyController)
    {
        enemy = _enemyController;
    }

    /// <summary>
    /// 攻撃可能設定
    /// </summary>
    public override void EnableHit() 
    {
        attackColliderV2 = enemy.AttackCollider;
        attackColliderV2.StartHit();

    
    }

    /// <summary>
    /// 攻撃不可設定
    /// </summary>
    public override void DisableHit()
    {
        attackColliderV2 = enemy.AttackCollider;
        attackColliderV2.EndHit();
    }

    /// <summary>
    /// 連撃フレーム
    /// </summary>
    public override void EnableCombo() { }

    /// <summary>
    /// 突進開始
    /// </summary>
    public override void StartDash()
    {
        EnemySkillManager skillManager = enemy.SkillManager;

        skillManager.DashHandler.Begin(true, enemy.transform.forward);
    }

    /// <summary>
    /// 突進終了
    /// </summary>
    public override void EndDash()
    {
        EnemySkillManager skillManager = enemy.SkillManager;

        skillManager.DashHandler.End();
    }

    public void TutorialJumpStopEvt()
    {
       
        {
            enemy.EnemyAction.TutorialStopTime();
        }
    }

    public void GenerateEffectEvt(int _index)
    {
        EffectHandler effectHandler = enemy.EffectHandler;
        effectHandler.GenerateEffect(_index);
      
    }
}
