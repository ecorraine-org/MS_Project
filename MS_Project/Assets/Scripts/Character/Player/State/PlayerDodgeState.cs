using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerState
{
    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(false);

        base.Init(_playerController);

        //方向、画像反転設定
        playerController.SetEightDirection();

        //入力方向取得
        //UnityEngine.Vector2 inputDirec = inputManager.GetMoveDirec();
        ////   方向設定
        //if (inputDirec == Vector2.zero) playerController.CurDirecVector = playerController.GetForward();

        playerSkillManager. ExecuteDodge(false, playerController.CurDirecVector);

        //無敵
        playerController.StatusManager.IsInvincible = true;
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


        //移動へ遷移
        Vector2 inputDirec=inputManager.GetMoveDirec();
        if (inputDirec.magnitude > 0 && !playerSkillManager.IsDashing)
        { 
            playerController.StateManager.TransitionState(StateType.Walk);
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
