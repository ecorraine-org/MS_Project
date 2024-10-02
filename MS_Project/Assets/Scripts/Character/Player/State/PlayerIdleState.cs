using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    //入力方向
    UnityEngine.Vector2 inputDirec;

    public override void Init(PlayerController _playerController)
    {
        base.Init(_playerController);

        // inputManager.BindAction(inputManager.InputControls.GamePlay.Attack, ExecuteAttack);

    }



    public override void Tick()
    {
        //ダメージチェック
        playerController.StateManager.CheckHit();

        //攻撃へ遷移
        bool isAttack = inputManager.GetAttackTrigger();
        if (isAttack) playerController.StateManager.TransitionState(StateType.Attack);

        //捕食へ遷移
        bool isEat = inputManager.GetEatTrigger();
        if (isEat) playerController.StateManager.TransitionState(StateType.Eat);

        //スキルへ遷移
        bool isSkill = inputManager.GetSkillTrigger();
        if (isSkill) playerController.StateManager.TransitionState(StateType.Skill);

        //アニメーション設定
        playerController.SetWalkAnimation();

        inputDirec = inputManager.GetMoveDirec();

        //移動へ遷移
        if (inputDirec.magnitude > 0)
            playerController.StateManager.TransitionState(StateType.Walk);

    }

    public override void FixedTick()
    {



    }

    public override void Exit()
    {

        // inputManager.UnBindAction(inputManager.InputControls.GamePlay.Attack);
    }



}
