using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAction : EnemyAction
{
    [SerializeField, Header("ターゲットレイヤー")]
    LayerMask targetLayer;

    //時間計測
    float frameTime = 0.0f;

    //移動を表す　0:歩く 1:走る
    int moveStage = 0;

    private Vector3 direction;

    #region Walk

    /// <summary>
    /// 移動処理初期化(一回だけ実行する)
    /// </summary>
    public void WalkInit()
    {
        Debug.Log("WalkInit");

        //初期化
        frameTime = 0.0f;

        //前の状態は歩きでなければ、初期化
        //歩きの場合、走りに変更した時、walkStageを1にする
        if (stateHandler.CurrentStateType != ObjectStateType.Walk) moveStage = 0;

        //歩き
        if (moveStage == 0) enemy.Anim.Play("Walk");
        //走り
        if (moveStage == 1) enemy.Anim.Play("Dash");

    }

    public void WalkTick()
    {
        frameTime++; //時間計測

        if (moveStage == 0) HandleWalk();
        if (moveStage == 1) HandleDash();

        Looking();

        //移動
        enemy.Move();

        //しばらく歩いたり、遠すぎると走りたいかも
        if (frameTime >= 600.0f ||
            distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 4.5f)
        {
            //投げよう
            //スキル状態へ遷移
            stateHandler.TransitionState(ObjectStateType.Skill);
            return;
        }


    }

    //WalkTickに呼び出される
    private void HandleWalk()
    {
        // 追跡
        enemy.OnMovementInput?.Invoke(direction.normalized);

        if (frameTime >= 240.0f &&
              distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 1.75f)
        {
            //移動状態(走り)へ遷移
            moveStage = 1;
            stateHandler.TransitionState(ObjectStateType.Walk);

            return;
        }

        //攻撃へ遷移
        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance && enemy.AllowAttack)
        {
            //クールダウン
            enemy.StartAttackCoroutine();

            stateHandler.TransitionState(ObjectStateType.Attack);
            return;
        }
    }

    //WalkTickに呼び出される
    private void HandleDash()
    {
        // 追跡
        enemy.OnMovementInput?.Invoke(direction.normalized * 5.0f);

        if (frameTime >= 60.0f && enemy.AllowAttack)//もう待ちきれない
        {

            //リセット(歩きに戻る)
            moveStage = 0;

            //クールダウン
            enemy.StartAttackCoroutine();

            //攻撃へ遷移
            stateHandler.TransitionState(ObjectStateType.Attack);
            return;

        }
    }
    #endregion

    #region Attack

    public void AttackInit()
    {
        animator.Play("Attack");

        frameTime = 0;
    }

    public void AttackTick()
    {
        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime <= 0.25f)
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.1f // 補間率（1.0fで即時、0.0fで変化なし）
        );

        //アニメーション終了
        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region ThrowSkill
    /// <summary>
    /// 投げ初期化
    /// </summary>
    public void SkillInit()
    {
        animator.Play("Throw");

        frameTime = 0.0f;
    }

    public void SkillTick()
    {
        //ちょっとずつ見る
        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Throw") && stateInfo.normalizedTime <= 0.4f)
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.25f // 補間率（1.0fで即時、0.0fで変化なし）
        );

        //アニメーション終了
        if (stateInfo.IsName("Throw") && stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    //見ているとこ見る
    public void Looking()
    {
        direction = player.position - enemy.transform.position;
        // 進む方向に向く
        Quaternion forwardRotation = Quaternion.LookRotation(direction.normalized);
        forwardRotation.x = 0f;
        enemy.transform.rotation = forwardRotation;
    }

    #region オノマトペ生成情報
    private void HunterWalkData()
    {
        GenerateWalkOnomatopoeia();
    }

    private void HunterAttackData()
    {
        GenerateAttackOnomatopoeia();
    }
    #endregion
}
