using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerState
{


    //入力方向
    UnityEngine.Vector2 inputDirec;

    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(false);

        base.Init(_playerController);
    }

    public override void Tick()
    {
        //ダメージチェック
        playerController.StateManager.CheckHit();

        //攻撃へ遷移
        bool isAttack = inputManager.GetAttackTrigger();
        if (isAttack)
        {
            playerController.StateManager.TransitionState(StateType.Attack);
            return;
        }

        //捕食へ遷移
        bool isEat = inputManager.GetEatTrigger();
        //フィニッシュ
        if (isEat&& playerController.DetectEnemy.CheckKillableEnemy())
        {
            playerController.StateManager.TransitionState(StateType.FinishSkill);
            return;
        }

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
        if (isSkill && playerController.SkillManager.CoolTimers[skill] <= 0
            && playerController.SkillManager.HpCost(skill))
        {
            playerController.StateManager.TransitionState(StateType.Skill);
            return;
        }

        //回避へ遷移
        if (playerStateManager.CheckDodge()) return;

        //方向設定
        playerController.SetEightDirection();

        //アニメーション設定
        playerController.SetWalkAnimation();

        //方向取得
        inputDirec = inputManager.GetMoveDirec();

        //アイドルへ遷移
        if (inputDirec.magnitude <= 0)
        {
            playerController.StateManager.TransitionState(StateType.Idle);
            return;
        }


    }

    public override void FixedTick()
    {
        //移動速度取得
        float moveSpeed = statusManager.StatusData.velocity;

        rb.velocity = new UnityEngine.Vector3(inputDirec.x * moveSpeed, rb.velocity.y, inputDirec.y * moveSpeed);
    }

    public override void Exit()
    {

    }
}
