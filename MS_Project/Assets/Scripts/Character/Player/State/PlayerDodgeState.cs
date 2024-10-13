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

        playerSkillManager.DashHandler.Dash(false);
    }

    public override void Tick()
    {
        //攻撃へ遷移
        bool isAttack = inputManager.GetAttackTrigger();
        if (isAttack)
        {
            playerController.StateManager.TransitionState(StateType.Attack);
            return;
        }

        //捕食へ遷移
        bool isEat = inputManager.GetEatTrigger();
        if (isEat
            && (playerController.SkillManager.CoolTimers[PlayerSkill.Eat] <= 0
            || playerController.StatusManager.IsFrenzy))
        {
            playerController.StateManager.TransitionState(StateType.Eat);
            return;
        }

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
