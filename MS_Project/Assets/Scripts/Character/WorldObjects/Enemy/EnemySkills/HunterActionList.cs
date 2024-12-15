using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterActionList : EnemyAction
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
        if (stateHandler.CheckHit()) return;

        //移動へ遷移
        float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distanceToPlayer <= enemyStatus.StatusData.chaseDistance)
        {
            enemy.State.TransitionState(ObjectStateType.Walk);
            return;
        }

        //リストへ遷移
        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance && CheckListTimer())
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
        frameTime += Time.deltaTime; //時間計測

        //ダメージチェック
        if (stateHandler.CheckHit()) return;

        if (moveStage == 0) HandleWalk();
        if (moveStage == 1) HandleDash();

        //移動
        enemy.Move();

    }

    //WalkTickに呼び出される
    private void HandleWalk()
    {
        // 追跡
        enemy.OnMovementInput?.Invoke(direction.normalized);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.3f // 補間率（1.0fで即時、0.0fで変化なし）
    );


        if (frameTime >= 1.8f ||
            distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 4.0f)
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
        //攻撃へ遷移遠め
        if (distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 2.8f && CheckListTimer())
        {
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

        currentUpdateAction = MoveTick;
    }

    public void MoveTick()
    {
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        if (moveStage == 0) HandleWalk();
        if (moveStage == 1) HandleDash();

        //移動
        enemy.Move();

    }
    #endregion

    #region Dash

    /// <summary>
    /// 移動処理初期化(一回だけ実行する)
    /// </summary>
    public void DashInit()
    {
        //初期化
        frameTime = 0.0f;
        moveStage = 1;
        //ダッシュ
        enemy.Anim.Play("Dash");

        currentUpdateAction = DashTick;
    }

    public void DashTick()
    {
        frameTime += Time.deltaTime;

        HandleDash();

        //移動
        enemy.Move();

    }
    //WalkTickやDashTickに呼び出される
    private void HandleDash()
    {
        // 追跡
        enemy.OnMovementInput?.Invoke(direction.normalized * 5.0f);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.3f // 補間率（1.0fで即時、0.0fで変化なし）
    );

        //攻撃へ遷移
        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance && CheckListTimer())
        {
            //リセット(歩きに戻る)
            moveStage = 0;
            stateHandler.TransitionState(ObjectStateType.Skill);//リストへ遷移
            return;
        }
        if (frameTime >= 1.5f ||
            distanceToPlayer <= enemyStatus.StatusData.attackDistance && CheckListTimer())
        {

            //リセット(歩きに戻る)
            moveStage = 0;

            //攻撃へ遷移
            stateHandler.TransitionState(ObjectStateType.Skill);//リストへ遷移
            return;

        }
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
        //攻撃判定
        //enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);
        //ダメージチェック
        //if (stateHandler.CheckHit()) return;

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
        enemy.OnMovementInput?.Invoke(direction.normalized * 0.0f);
        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = AttackTick;
    }

    public void AttackTick()
    {
        //ダメージチェック
        //if (stateHandler.CheckHit()) return;
        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        //移動
        enemy.Move();

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        if (stateInfo.normalizedTime <= 0.25f)
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.1f // 補間率（1.0fで即時、0.0fで変化なし）
        );

        if (stateInfo.normalizedTime <= 0.2f)
        {
            // 前に進行
            float chargeForce = enemy.RigidBody.mass * 11.0f;
            enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);

        }

        //アニメーション終了
        if (stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Attack_Dual

    /// <note>
    /// 関数名は「Attack」や「Skill」にならないように
    /// </note>
    public void Attack_DualInit()
    {

        animator.Play("Attack_Dual");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = Attack_DualTick;
    }

    public void Attack_DualTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(10.0f, false);
        //ダメージチェック
        //if (stateHandler.CheckHit()) return;

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime <= 0.1f)
        {
            // 前に進行
            float chargeForce = enemy.RigidBody.mass * 14.0f;
            enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);

        }
        else if (stateInfo.normalizedTime >= 0.32f && stateInfo.normalizedTime <= 0.4f)
        {
            // 前に進行
            float chargeForce = enemy.RigidBody.mass * 20.0f;
            enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);

        }
        else if(stateInfo.normalizedTime <= 0.32f)
        {
            direction = player.position - enemy.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            targetRotation.x = 0f;
            targetRotation.z = 0f;

            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.2f // 補間率（1.0fで即時、0.0fで変化なし）
        );
        }

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
