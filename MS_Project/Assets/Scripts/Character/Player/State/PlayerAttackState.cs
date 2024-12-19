using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    [SerializeField, Header("ヒットリアクションデータ")]
    PlayerHitData playerHitData;

    [SerializeField, Header("コライダー")]
    HitCollider hitCollider;

    //受付時間内にボタンを押すと、次のコンボが発動する
    bool willNextStage = false;

    public float FrenzyAttackDamage;

    private CameraBasedHitCorrection _CameraBasedHitCorrection;


    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(true);

        base.Init(_playerController);

        playerController.BattleManager.slowSpeed = playerHitData.dicHitReac[playerModeManager.Mode].slowSpeed;
        playerController.BattleManager.stopDuration = playerHitData.dicHitReac[playerModeManager.Mode].stopDuration;

        playerController.AttackColliderV2.HitCollidersList = hitCollider;

        //コンポーネントの取得
        _CameraBasedHitCorrection = GetComponent<CameraBasedHitCorrection>();

        //方向変更
        playerController.SetEightDirection();

        //前の状態は攻撃でなければ、攻撃段階リセット
        if (playerStateManager.PreStateType != StateType.Attack) playerSkillManager.AttackStage = 0;

        switch (playerModeManager.Mode)
        {
            case PlayerMode.None:
                playerSkillManager.AttackDamage = 1;
                spriteAnim.Play("Attack", 0, 0f);
                break;
            case PlayerMode.Sword:
                playerSkillManager.SwordAttackInit();
                break;
            case PlayerMode.Hammer:
                playerSkillManager.HammerAttackInit();

                break;
            case PlayerMode.Spear:
                playerSkillManager.SpearAttackInit();
                break;
            case PlayerMode.Gauntlet:
                playerSkillManager.GauntletAttackInit();


                break;
            default:
                break;
        }

        //仮のクールタイマー初期化
        // testTimer = 0;
    }

    public override void Tick()
    {

        //コンボ受付
        if (playerSkillManager.CanComboInput && playerController.InputManager.GetAttackTrigger())
        {
            playerSkillManager.CanComboInput = false;

            //キャンセルフレームに達したら、コンボする
            willNextStage = true;
        }

        //自身へ遷移
        if (willNextStage && playerSkillManager.CanComboCancel)
        {
            playerSkillManager.CanComboCancel = false;
            willNextStage = false;

            playerSkillManager.NextAttack();

            return;
        }


        //回避へ遷移
        if (playerStateManager.CheckDodge()) return;

        //ダメージチェック
        if (playerStateManager.CheckHit()) return;

        //スキルへ遷移
        if (playerStateManager.CheckSkill()) return;

        //捕食へ遷移
        if (playerStateManager.CheckEat()) return;

        Attack();

        //アニメーション終了、アイドルへ遷移
        if (spriteAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            playerController.StateManager.TransitionState(StateType.Idle);
        }
    }

    public override void FixedTick()
    {

    }

    public override void Exit()
    {
        willNextStage = false;
    }

    public void Attack()
    {

        //仮処理
        float damage = 0;
        if (statusManager.IsFrenzy) damage = FrenzyAttackDamage;
        else damage = playerSkillManager.AttackDamage * buffManager.BuffEffect.damageUpRate;
        //コライダーの検出
        playerController.AttackColliderV2.DetectColliders(damage, false);

    }

    /// <summary>
    /// 描画test
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (_CameraBasedHitCorrection != null)
        {
            //  _CameraBasedHitCorrection.VisualizeCollider(attackAreaPos, attackSize, false);
        }
        else
        {
            // Gizmos.color = Color.yellow;
            // Gizmos.DrawWireCube(attackAreaPos, attackSize);
        }
    }

}



