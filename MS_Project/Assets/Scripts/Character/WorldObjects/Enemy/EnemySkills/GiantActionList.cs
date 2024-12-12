using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantActionList : EnemyAction
{
    [SerializeField, Header("ターゲットレイヤー")]
    LayerMask targetLayer;

    //時間計測
    float frameTime = 0.0f;

    //移動を表す　0:歩く 1:走る
    int moveStage = 0;

    private Vector3 direction;

    #region Idle
    public void IdleInit()
    {
        listTimer = 0;

        enemy.Anim.Play("Idle");
    }
    public void IdleTick()
    {
        //nullを防止するため、再取得する
        stateHandler = enemy.State;

        if (stateHandler.CheckDeath()) return;

        //ダメージチェック
        //if (stateHandler.CheckHit()) return;

        //移動へ遷移
        float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distanceToPlayer <= enemyStatus.StatusData.chaseDistance)
        {
            enemy.State.TransitionState(ObjectStateType.Walk);
            return;
        }

        //リストへ遷移
        if (CheckListTimer())
        {
            enemy.State.TransitionState(ObjectStateType.Skill);
            return;
        }

    }
    #endregion

    #region Walk

    /// <summary>
    /// 移動処理初期化(一回だけ実行する)
    /// </summary>
    public void WalkInit()
    {
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
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime; //時間計測

        if (moveStage == 0) HandleWalk();
        if (moveStage == 1) HandleDash();

        Looking();

        //移動
        enemy.Move();

    }

    //WalkTickに呼び出される
    private void HandleWalk()
    {
        // 追跡
        enemy.OnMovementInput?.Invoke(direction.normalized);

        if (frameTime >= 5.0f &&
              distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 3.0f ||
              distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 5.0f)
        {
            //移動状態(走り)へ遷移
            moveStage = 1;
            stateHandler.TransitionState(ObjectStateType.Walk);

            return;
        }

        //攻撃へ遷移
        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance && CheckListTimer())
        {
            stateHandler.TransitionState(ObjectStateType.Skill);//リストへ遷移
            return;
        }
    }
    #endregion

    #region Dash
    public void DashInit()
    {
        //初期化
        frameTime = 0.0f;

        //前の状態は歩きでなければ、初期化
        //歩きの場合、走りに変更した時、walkStageを1にする
        if (stateHandler.CurrentStateType != ObjectStateType.Walk) moveStage = 0;

        //走り
        enemy.Anim.Play("Dash");

    }

    public void DashTick()
    {
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime; //時間計測

        HandleDash();        

        //移動
        enemy.Move();

    }

    //DashTickやWalkTickに呼び出される
    private void HandleDash()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //攻撃判定
        enemy.AttackCollider.DetectColliders(30.0f, false);

        if (!stateInfo.IsName("Dash_Attack"))
        {
            enemy.Anim.Play("Dash");
        }

        if(stateInfo.IsName("Dash"))
        {
            // 追跡
            enemy.OnMovementInput?.Invoke(direction.normalized * 10.0f);
            Looking();
            if (frameTime >= 4.0f ||
            distanceToPlayer <= enemyStatus.StatusData.attackDistance * 1.5f && CheckListTimer())//もう待ちきれない  
            {
                    //ダッシュ攻撃へ遷移
                    enemy.Anim.Play("Dash_Attack");
                    //stateHandler.TransitionState(ObjectStateType.Skill);//リストへ遷移
             }
        }

        if (stateInfo.IsName("Dash_Attack"))
        {
            // 追跡
            enemy.OnMovementInput?.Invoke(direction.normalized * 0.0f);
            if (stateInfo.normalizedTime >= 1.0f)
            {
                //リセット(歩きに戻る)
                moveStage = 0;

                stateHandler.TransitionState(ObjectStateType.Idle);
            }
        }
    }
    #endregion

    #region Attack

    /// <note>
    /// 関数名は「Attack」や「Skill」にならないように
    /// </note>
    public void AttackInit()
    {

        animator.Play("Attack");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = AttackTick;
    }

    public void AttackTick()
    {
        if (stateHandler.CheckDeath()) return;

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        if (stateInfo.normalizedTime <= 0.5f)
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.05f // 補間率（1.0fで即時、0.0fで変化なし）
        );

        //アニメーション終了
        if (stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Attack_Side
    /// <summary>
    /// 投げ初期化
    /// </summary>
    public void Attack_SideInit()
    {
        animator.Play("Attack_Side");

        frameTime = 0.0f;

        currentUpdateAction = Attack_SideTick;
    }

    public void Attack_SideTick()
    {
        if (stateHandler.CheckDeath()) return;

        //ちょっとずつ見る
        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        if (stateInfo.normalizedTime <= 0.4f)
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.01f // 補間率（1.0fで即時、0.0fで変化なし）
        );

        //アニメーション終了
        if (stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region ActionList

    /// <summary>
    /// 初期化
    /// </summary>
    public void SkillInit()
    {

        actionPatternList[listIndex].GetActionPattern()[actionStage].OnAction?.Invoke();


    }

    public void SkillTick()
    {
        //バインドした関数を呼び出す
        currentUpdateAction?.Invoke();
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
