using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateActionList : EnemyAction
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

        moveStage = 0;

        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        //ダメージチェック
        //if (stateHandler.CheckHit()) return;

        //移動へ遷移
        float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);

        if (distanceToPlayer <= enemyStatus.StatusData.chaseDistance &&
            distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 0.3f)
        {
            if (frameTime >= 0.2f)
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

        //移動状態(走り)へ遷移
        if (distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 3.0f ||
              frameTime >= 4.0f)
        {
            moveStage = 1;
            stateHandler.TransitionState(ObjectStateType.Walk);

            return;
        }

        //攻撃へ遷移近い
        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance && CheckListTimer())
        {
            stateHandler.TransitionState(ObjectStateType.Skill);//リストへ遷移
            return;
        }
        //攻撃へ遷移遠め
        if (distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 2.0f && CheckListTimer())
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
        if (stateHandler.CheckDeath()) return;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (frameTime >= 5.0f)//もう待ちきれない  
        {
            //攻撃へ遷移
            stateHandler.TransitionState(ObjectStateType.Skill);//リストへ遷移
            return;

        }

        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance / 2 || frameTime >= 1.5f)
        {//ダッシュ攻撃
            enemy.Anim.Play("Dash_Attack");
            //攻撃判定
            enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);
            // 追跡
            enemy.OnMovementInput?.Invoke(direction.normalized * 0.3f);
        }
        if (!stateInfo.IsName("Dash_Attack"))
        {
            Looking();
            moveStage = 1;
            //ダッシュ
            enemy.Anim.Play("Dash");
            // 追跡
            enemy.OnMovementInput?.Invoke(direction.normalized * 18.0f);
        }

        if (stateInfo.IsName("Dash_Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }

    }
    #endregion

    #region Back_Step

    public void Back_StepInit()
    {

        animator.Play("Back_Step");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = Back_StepTick;
    }

    public void Back_StepTick()
    {
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

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
        {
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.1f // 補間率（1.0fで即時、0.0fで変化なし）
        );

            if (stateInfo.IsName("Back_Step"))
            {
                Vector3 forceDirection = -enemy.transform.forward * 18.0f; // 後ろ方向の力
                enemy.GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.VelocityChange);
            }
        }

        //フィニッシュ攻撃
        if (stateInfo.IsName("Back_Step") && stateInfo.normalizedTime >= 1.0f)
        {
            animator.Play("Slash");
            enemy.OnMovementInput?.Invoke(direction.normalized * 0.1f);
        }
        //アニメーション終了
        if (stateInfo.IsName("Slash") && stateInfo.normalizedTime >= 1.0f)
            stateHandler.TransitionState(ObjectStateType.Idle);
    }
    #endregion

    #region Slash

    public void SlashInit()
    {

        animator.Play("Slash");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = SlashTick;
    }

    public void SlashTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

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

        //アニメーション終了
        if (stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Slash_Dual

    public void Slash_DualInit()
    {

        animator.Play("Slash_Dual");
        enemy.OnMovementInput?.Invoke(direction.normalized * 0.0f);
        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = Slash_DualTick;
    }

    public void Slash_DualTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(12.0f, false);

        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime <= 0.3f)
        {
            direction = player.position - enemy.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            targetRotation.x = 0f;
            targetRotation.z = 0f;

            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.3f // 補間率（1.0fで即時、0.0fで変化なし）
        );
        }
        else if ((stateInfo.normalizedTime > 0.3f && stateInfo.normalizedTime <= 0.5f))
        {
            // 前に進行
            float chargeForce = enemy.RigidBody.mass * 1.0f;
            enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);
        }

        //移動
        enemy.Move();

        //アニメーション終了
        if (stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Slash_Triple

    public void Slash_TripleInit()
    {

        animator.Play("Slash_Triple");
        enemy.OnMovementInput?.Invoke(direction.normalized * 0.0f);
        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = Slash_TripleTick;
    }

    public void Slash_TripleTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(10.0f, false);

        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);


        if (stateInfo.normalizedTime <= 0.3f)
        {
            direction = player.position - enemy.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            targetRotation.x = 0f;
            targetRotation.z = 0f;

            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.3f // 補間率（1.0fで即時、0.0fで変化なし）
        );
        }
        else if (stateInfo.normalizedTime > 0.3f && stateInfo.normalizedTime <= 0.65f)
        {
            direction = player.position - enemy.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            targetRotation.x = 0f;
            targetRotation.z = 0f;

            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.02f // 補間率（1.0fで即時、0.0fで変化なし）
        );

            // 前に進行
            float chargeForce = enemy.RigidBody.mass * 0.7f;
            enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);
        }

        //移動
        enemy.Move();

        //アニメーション終了
        if (stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Slash_Jump

    public void Slash_JumpInit()
    {

        animator.Play("Slash_Jump_Start");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = Slash_JumpTick;
    }

    public void Slash_JumpTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;
        //移動
        enemy.Move();


        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Slash_Jump_Start") && stateInfo.normalizedTime <= 0.27f)
        {
            direction = player.position - enemy.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            targetRotation.x = 0f;
            targetRotation.z = 0f;

            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.15f // 補間率（1.0fで即時、0.0fで変化なし）
        );
        }
        else if(stateInfo.IsName("Slash_Jump_Start"))
        {
            // 前に進行
            float chargeForce = enemy.RigidBody.mass * 12.0f;
            enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);

            // 追跡
            //enemy.OnMovementInput?.Invoke(direction.normalized * 18.0f);
        }

        //フィニッシュ攻撃
        if (frameTime >= 0.6f)
        {
            animator.Play("Slash_Jump_End");

            enemy.OnMovementInput?.Invoke(direction.normalized * 0.5f);

            //アニメーション終了
            if (stateInfo.IsName("Slash_Jump_End") && stateInfo.normalizedTime >= 1.0f)
                stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Sting

    public void StingInit()
    {

        animator.Play("Sting");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = StingTick;
    }

    public void StingTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(22.0f, false);

        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime <= 0.33f)
        {
            enemy.OnMovementInput?.Invoke(direction.normalized * 0.0f);

            direction = player.position - enemy.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            targetRotation.x = 0f;
            targetRotation.z = 0f;

            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.3f // 補間率（1.0fで即時、0.0fで変化なし）
        );
        }
        else if (stateInfo.normalizedTime > 0.33f && stateInfo.normalizedTime <= 0.65f)
        {
            // 追跡
            enemy.OnMovementInput?.Invoke(direction.normalized * 23.0f);
        }
        else
        {
            enemy.OnMovementInput?.Invoke(direction.normalized * 2.3f);
        }

        //移動
        enemy.Move();

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
        frameTime += Time.deltaTime;
        Debug.Log(frameTime);
        //移動
        enemy.Move();        

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //回ってる間は追跡
        if (stateInfo.IsName("Spin_Start"))
        {
            enemy.OnMovementInput?.Invoke(direction.normalized * 0.0f);

            //ちょっとずつ見る
            direction = player.position - enemy.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            targetRotation.x = 0f;
            targetRotation.z = 0f;

            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.3f // 補間率（1.0fで即時、0.0fで変化なし）
        );
        }
        else if (stateInfo.IsName("Spin"))
        {//回ってる間は突進
            //ちょっとずつ見る
            direction = player.position - enemy.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            targetRotation.x = 0f;
            targetRotation.z = 0f;

            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.02f // 補間率（1.0fで即時、0.0fで変化なし）
        );
            //攻撃判定
            enemy.AttackCollider.DetectColliders(5.0f, false);
            // 追跡
            //enemy.OnMovementInput?.Invoke(direction.normalized * 4.5f);
            // 前に進行
            float chargeForce = enemy.RigidBody.mass * 1.5f;
            enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);

        }

        //フィニッシュ攻撃
        if (frameTime >= 1.4f)
        {
            // 前に進行
            float chargeForce = enemy.RigidBody.mass * 0.15f;
            enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);

            //攻撃判定
            enemy.AttackCollider.DetectColliders(10.0f, false);
            animator.Play("Dash_Attack");

            if (stateInfo.IsName("Dash_Attack") && stateInfo.normalizedTime >= 1.0f)
                stateHandler.TransitionState(ObjectStateType.Idle);
        }
        if (stateHandler.CheckDeath()) return;

    }
    #endregion

    #region Charge_Slash

    public void Charge_SlashInit()
    {

        animator.Play("Charge");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = Charge_SlashTick;
    }

    public void Charge_SlashTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(36.0f, false);

        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        //フィニッシュ攻撃
        if (frameTime >= 0.5f)
        {
            animator.Play("Charge_Slash");

            enemy.OnMovementInput?.Invoke(direction.normalized * 0.5f);

            if (stateInfo.normalizedTime <= 0.25f)
                enemy.transform.rotation = Quaternion.Slerp(
                enemy.transform.rotation,
                targetRotation,
                0.1f // 補間率（1.0fで即時、0.0fで変化なし）
            );

            //アニメーション終了
            if (stateInfo.IsName("Charge_Slash") && stateInfo.normalizedTime >= 1.0f)
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

    public void VelFwd_UL()
    {
        // 前に進行
        float chargeForce = enemy.RigidBody.mass * 3.0f;
        enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);
    }

    public void Smooth_Look()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);


        enemy.OnMovementInput?.Invoke(direction.normalized * 0.0f);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.4f // 補間率（1.0fで即時、0.0fで変化なし）
    );
    }

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
