using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemActionList : EnemyAction
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
        frameTime += Time.deltaTime;

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
        //if (moveStage == 1) enemy.Anim.Play("Dash");

    }

    public void WalkTick()
    {
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

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

        if (frameTime >= 240.0f &&
              distanceToPlayer >= EnemyStatus.StatusData.chaseDistance)
        {
            //移動状態(走り)へ遷移
            moveStage = 1;
            stateHandler.TransitionState(ObjectStateType.Walk);

            return;
        }

        //攻撃へ遷移
        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance * 6.0f && CheckListTimer())
        {
            stateHandler.TransitionState(ObjectStateType.Skill);//リストへ遷移
            return;
        }
    }

    //WalkTickに呼び出される
    private void HandleDash()
    {
        // 追跡
        enemy.OnMovementInput?.Invoke(direction.normalized * 5.0f);

        if (frameTime >= 60.0f && CheckListTimer())//もう待ちきれない  
        {

            //リセット(歩きに戻る)
            moveStage = 0;

            //攻撃へ遷移
            stateHandler.TransitionState(ObjectStateType.Skill);//リストへ遷移
            return;

        }
    }
    #endregion

    #region Move

    /// <summary>
    /// 移動処理初期化(一回だけ実行する)
    /// </summary>
    public void MoveInit()
    {
        //初期化
        frameTime = 0.0f;

        //前の状態は歩きでなければ、初期化
        //歩きの場合、走りに変更した時、walkStageを1にする
        if (stateHandler.CurrentStateType != ObjectStateType.Walk) moveStage = 0;

        //歩き
        if (moveStage == 0) enemy.Anim.Play("Walk");
        //走り
        //if (moveStage == 1) enemy.Anim.Play("Dash");
        currentUpdateAction = MoveTick;
    }

    public void MoveTick()
    {
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        if (moveStage == 0) HandleWalk();
        if (moveStage == 1) HandleDash();

        Looking();

        //移動
        enemy.Move();

    }
    #endregion

    #region ThrowSkill
    /// <summary>
    /// 投げ初期化
    /// </summary>
    public void ThrowSkillInit()
    {
        animator.Play("Throw");

        frameTime = 0.0f;

        currentUpdateAction = ThrowSkillTick;
    }

    public void ThrowSkillTick()
    {
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);


        //ちょっとずつ見る
        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime <= 0.4f)
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.25f // 補間率（1.0fで即時、0.0fで変化なし）
        );

        //アニメーション終了
        if (stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Spin
    /// <summary>
    /// 投げ初期化
    /// </summary>
    public void SpinInit()
    {
        animator.Play("Spin_Start");

        frameTime = 0.0f;

        currentUpdateAction = SpinTick;
    }

    public void SpinTick()
    {
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;
        //Debug.Log(frameTime);
        //移動
        enemy.Move();

        //攻撃判定
        enemy.AttackCollider.DetectColliders(5.0f, false);


        //ちょっとずつ見る
        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //回ってる間は追跡
        if (stateInfo.IsName("Spin_Start"))
        {
            // 追跡
            enemy.OnMovementInput?.Invoke(direction.normalized);

            if (stateInfo.normalizedTime <= 0.4f)
                enemy.transform.rotation = Quaternion.Slerp(
                enemy.transform.rotation,
                targetRotation,
                0.25f // 補間率（1.0fで即時、0.0fで変化なし）
            );
        }

        //アニメーション終了からのフィニッシュ攻撃
        //if (stateInfo.normalizedTime >= 1.0f)
        if (frameTime >= 4.0f)
        {
            animator.Play("Spin_End");

            enemy.OnMovementInput?.Invoke(direction.normalized * 0.5f);

            if (stateInfo.IsName("Spin_End") && stateInfo.normalizedTime >= 1.0f)
                stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Attack2
    /// <summary>
    /// 投げ初期化
    /// </summary>
    public void AttackTwoInit()
    {
        animator.Play("Attack2");

        frameTime = 0.0f;

        currentUpdateAction = AttackTwoTick;
    }

    public void AttackTwoTick()
    {
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        //攻撃判定
        enemy.AttackCollider.DetectColliders(20.0f, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

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
        if (distanceToPlayer >= EnemyStatus.StatusData.attackDistance)
        {
            //遠距離リスト指定
            listIndex = 1;
        }
        else
        {
            //近距離リスト指定
            listIndex = 0;
        }

        actionPatternList[listIndex].GetActionPattern()[actionStage].OnAction?.Invoke();


    }

    public void SkillTick()
    {
        //バインドした関数を呼び出す
        currentUpdateAction?.Invoke();
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
        frameTime += Time.deltaTime;

        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);


        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        if (stateInfo.normalizedTime <= 0.25f)
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.1f // 補間率（1.0fで即時、0.0fで変化なし）
        );

        //アニメーション終了
        if (stateInfo.normalizedTime >= 1.0f)
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
