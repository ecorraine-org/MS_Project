using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    //入力方向
    UnityEngine.Vector2 inputDirec;

    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(false);

        base.Init(_playerController);

      // Debug.Log("アイドル状態");

        spriteAnim.Play("Idle", 0, 0f);
    }



    public override void Tick()
    {
        //ダメージチェック
        if (playerController.StateManager.CheckHit()) return;

        //攻撃へ遷移
        if (playerStateManager.CheckAttack()) return;

        //捕食へ遷移
        if (playerStateManager.CheckEat()) return;
 
        //スキルへ遷移
        if (playerStateManager.CheckSkill()) return;

        //回避へ遷移
        if (playerStateManager.CheckDodge()) return;


        //アニメーション設定
      //  if (!playerSkillManager.IsDashing) playerController.SetWalkAnimation();

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
