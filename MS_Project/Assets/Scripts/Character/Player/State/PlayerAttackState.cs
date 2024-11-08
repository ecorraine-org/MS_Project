using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    [SerializeField, Header("コライダー")]
    HitCollider hitCollider;

    //攻撃test
    [SerializeField, Header("攻撃力")]
    float attackDamage;
    public float FrenzyAttackDamage;
    public LayerMask enemyLayer;
    private CameraBasedHitCorrection _CameraBasedHitCorrection;

    //仮処理
    //private float testTimer;

    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(true);

        base.Init(_playerController);

        playerController.AttackColliderV2.HitCollidersList = hitCollider;

        //コンポーネントの取得
        _CameraBasedHitCorrection = GetComponent<CameraBasedHitCorrection>();

        //方向変更
        playerController.SetEightDirection();

       

        switch (playerModeManager.Mode)
        {
            case PlayerMode.None:
                attackDamage = 1;
                spriteAnim.Play("Attack",0,0f);
                break;
            case PlayerMode.Sword:
                attackDamage = statusManager.StatusData.swordAtk;
                spriteAnim.Play("Attack", 0, 0f);
                break;
            case PlayerMode.Hammer:
                attackDamage = statusManager.StatusData.hammerAtk;
                spriteAnim.Play("HammerAttack", 0, 0f);
                break;
            case PlayerMode.Spear:
                attackDamage = statusManager.StatusData.spearAtk;
                spriteAnim.Play("SpearAttack", 0, 0f);
                break;
            case PlayerMode.Gauntlet:
                attackDamage = statusManager.StatusData.gauntletAtk;
                spriteAnim.Play("GauntletAttack", 0, 0f);
                break;
            default:
                break;
        }

        //仮のクールタイマー初期化
        // testTimer = 0;
    }

    public override void Tick()
    {
        //自身へ遷移
        if ( playerSkillManager.CanCombo && playerStateManager.CheckAttack())
        {
            playerSkillManager.CanCombo = false;
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
    }

    public void Attack()
    {

        //仮処理
        float damage = 0;
        if (statusManager.IsFrenzy) damage = FrenzyAttackDamage;
        else damage = attackDamage;
        //コライダーの検出
        playerController.AttackColliderV2.DetectColliders(  damage, enemyLayer, false);

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



