using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerState
{
    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(false);

        base.Init(_playerController);
        Debug.Log("回避ステート");

        //方向、画像反転設定
        playerController.SetEightDirection();
        playerSkillManager. ExecuteDodge(false, playerController.CurDirecVector);
    }

    public override void Tick()
    {
        //攻撃へ遷移
        if (playerStateManager.CheckAttack()) return;

        //捕食へ遷移
        if (playerStateManager.CheckEat()) return;

        //スキルへ遷移
        bool isSkill = inputManager.GetSkillTrigger();
        PlayerSkill skill = (PlayerSkill)(int)playerController.ModeManager.Mode;
        if (isSkill && playerController.SkillManager.CoolTimers[skill] <= 0)
        {
            playerController.StateManager.TransitionState(StateType.Skill);
            return;
        }

        //アイドルへ遷移
        if (!playerSkillManager.IsDashing)
        {
            playerStateManager.TransitionState(StateType.Idle);
            return;
        }
     
       

    }

    public override void FixedTick()
    {

    }

    public override void Exit()
    {
        
    }
}
