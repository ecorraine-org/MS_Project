using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiabolosActionListV2 : EnemyAction
{
    [SerializeField, Header("ターゲットレイヤー")]
    LayerMask targetLayer;

    //やってまいりました
    bool isEntry = false;
    //時間計測
    float frameTime = 0.0f;
    //状態
    int stateType = 0;

    //移動を表す　0:歩く 1:走る
    int moveStage = 0;

    private Vector3 direction;

    #region Died
    public void DiedInit()
    {
        enemy.Anim.Play("Died", 0, 0.0f);
    }
    public void DiedTick()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //後ろへ
        if (stateInfo.normalizedTime < 1.0f)
        {
            Vector3 forceDirection = -enemy.transform.forward * 0.3f; // 後ろ方向の力
            enemy.GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.VelocityChange);
        }


    }
    #endregion


    #region Idle
    public void IdleInit()
    {
        listTimer = 0;

        //戦場にエントリー
        if (isEntry)
        {
            if (stateType == 0)
                enemy.Anim.Play("Idle");
            else
                enemy.Anim.Play("Idle_C");
        }
        else
        {
            enemy.Anim.Play("Entry");
        }
    }
    public void IdleTick()
    {
        //nullを防止するため、再取得する
        stateHandler = enemy.State;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //開幕挑発
        if (!isEntry)
        {
            if (stateInfo.normalizedTime >= 1.0f)
                isEntry = true;
            else if (stateInfo.normalizedTime <= 0.01f)
                Looking();

            return;
        }else if (stateType == 0)
        {//立ち状態へ
            if (stateInfo.IsName("Idle_C"))
                enemy.Anim.Play("C_to_S");
        }
        else if (stateType == 1)
        {//屈み状態へ
            if (stateInfo.IsName("Idle"))
                enemy.Anim.Play("S_to_C");
        }


        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        //ダメージチェック
        //if (stateHandler.CheckHit()) return;

        //リストへ遷移
        if ((stateInfo.IsName("Idle") || stateInfo.IsName("Idle_C")) && CheckListTimer())
        {
            enemy.State.TransitionState(ObjectStateType.Skill);
            return;
        }

    }
    #endregion

    //立ち中----------------------------------------------------------

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

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if ((!stateInfo.IsName("Stand") || !stateInfo.IsName("Crouch")))
        {
            if (stateType == 0) HandleWalk();
            if (stateType == 1) HandleDash();
        }

        //移動
        enemy.Move();

    }

    //WalkTickに呼び出される
    private void HandleWalk()
    {
        if (Stand()) return;
        else animator.Play("Walk");


        // 追跡
        enemy.OnMovementInput?.Invoke(direction.normalized);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.2f // 補間率（1.0fで即時、0.0fで変化なし）
    );

        //移動状態(走り)へ遷移
        if (distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 4.0f ||
              frameTime >= 2.0f)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            moveStage = 1;
            stateType = 1;
            enemy.Anim.Play("Crouch");
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
        if (distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 4.0f && CheckListTimer())
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

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        {
            if (stateType == 0) HandleWalk();
            if (stateType == 1) HandleDash();
        }

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
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (Crouch()) return;

        if (stateHandler.CheckDeath()) return;

        if (frameTime >= 5.0f)//もう待ちきれない  
        {
            //攻撃へ遷移
            stateHandler.TransitionState(ObjectStateType.Skill);//リストへ遷移
            return;

        }

        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance || frameTime >= 1.5f)
        {//ダッシュ攻撃
            enemy.Anim.Play("Slap");
            //攻撃判定
            enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);
            // 追跡
            enemy.OnMovementInput?.Invoke(direction.normalized * 0.3f);
        }
        if (!stateInfo.IsName("Slap"))
        {
            Looking();
            moveStage = 1;
            //ダッシュ
            enemy.Anim.Play("Dash");
            // 追跡
            enemy.OnMovementInput?.Invoke(direction.normalized * 3.0f);
        }

        if (stateInfo.IsName("Slap") && stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }

    }
    #endregion

    #region Attack

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
        //攻撃判定
        enemy.AttackCollider.DetectColliders(12.0f, false);

        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        if (Stand()) return;
        else animator.Play("Attack");

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
            0.2f // 補間率（1.0fで即時、0.0fで変化なし）
        );
        }
        else if ((stateInfo.normalizedTime > 0.3f && stateInfo.normalizedTime <= 0.5f))
        {
            // 前に進行
            float chargeForce = enemy.RigidBody.mass * 2.0f;
            enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);
        }

        //移動
        enemy.Move();

        //アニメーション終了
        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Bite

    /// <note>
    /// 関数名は「Attack」や「Skill」にならないように
    /// </note>
    public void BiteInit()
    {

        animator.Play("Bite");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = BiteTick;
    }

    public void BiteTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        //死んでいるかと時間計測
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        if (Stand()) return;
        else animator.Play("Bite");

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
        0.2f // 補間率（1.0fで即時、0.0fで変化なし）
    );
        }
        else if ((stateInfo.normalizedTime > 0.25f && stateInfo.normalizedTime <= 0.35f))
        {
            // 前に進行
            float chargeForce = enemy.RigidBody.mass * 2.5f;
            enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);
        }

        //アニメーション終了
        if (stateInfo.IsName("Bite") && stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Tail
    /// <summary>
    /// 投げ初期化
    /// </summary>
    public void TailInit()
    {
        animator.Play("Tail");

        frameTime = 0.0f;

        currentUpdateAction = TailTick;
    }

    public void TailTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(18.0f, false);

        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        if (Stand()) return;
        else animator.Play("Tail");


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
        }
        else if ((stateInfo.normalizedTime > 0.25f && stateInfo.normalizedTime <= 0.45f))
        {
            // 前に進行
            float chargeForce = enemy.RigidBody.mass * 1.5f;
            enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);
        }

        if (stateInfo.IsName("Tail") && stateInfo.normalizedTime >= 1.0f)
            stateHandler.TransitionState(ObjectStateType.Idle);
    }
    #endregion

    #region Axe

    public void AxeInit()
    {
        if (stateType == 0)
            animator.Play("Axe_Start");
        else
            animator.Play("C_to_S");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = AxeTick;
    }

    public void AxeTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (Stand()) return;
        else if (!stateInfo.IsName("Axe_Start") && !stateInfo.IsName("Axe_End")) animator.Play("Axe_Start");

        //移動
        enemy.Move();


        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);


        if (stateInfo.IsName("Axe_Start") && stateInfo.normalizedTime <= 0.8f)
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
        else if (stateInfo.IsName("Axe_Start"))
        {
            // 前に進行
            float chargeForce = enemy.RigidBody.mass * 2.5f;
            enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);

            // 追跡
            //enemy.OnMovementInput?.Invoke(direction.normalized * 18.0f);
        }

        //フィニッシュ攻撃
        if (stateInfo.IsName("Axe_End"))
        {
            //animator.Play("Axe_End");

            enemy.OnMovementInput?.Invoke(direction.normalized * 0.5f);

            //アニメーション終了
            if (stateInfo.IsName("Axe_End") && stateInfo.normalizedTime >= 1.0f)
                stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    //屈み中==========================================================

    #region Slap

    /// <note>
    /// 関数名は「Attack」や「Skill」にならないように
    /// </note>
    public void SlapInit()
    {

        animator.Play("Slap");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = SlapTick;
    }

    public void SlapTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        //死んでいるかと時間計測
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //アニメーション終了
        if (stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Cannon

    /// <note>
    /// 関数名は「Attack」や「Skill」にならないように
    /// </note>
    public void CannonInit()
    {

        animator.Play("Cannon");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = CannonTick;
    }

    public void CannonTick()
    {
        //死んでいるかと時間計測
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;


        if (Crouch()) return;
        else animator.Play("Cannon");

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        if (stateInfo.normalizedTime <= 0.4f)
            enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.2f // 補間率（1.0fで即時、0.0fで変化なし）
    );

        //アニメーション終了
        if (stateInfo.IsName("Cannon") && stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Breath

    /// <note>
    /// 関数名は「Attack」や「Skill」にならないように
    /// </note>
    public void BreathInit()
    {

        animator.Play("Breath");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = BreathTick;
    }

    public void BreathTick()
    {
        //死んでいるかと時間計測
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        if (Crouch()) return;
        else animator.Play("Breath");

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        if (stateInfo.normalizedTime <= 0.2f)
            enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.2f // 補間率（1.0fで即時、0.0fで変化なし）
    );

        //アニメーション終了
        if (stateInfo.IsName("Breath") && stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    //--------------
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



    //見ているとこ見る
    public void Looking()
    {
        direction = player.position - enemy.transform.position;
        // 進む方向に向く
        Quaternion forwardRotation = Quaternion.LookRotation(direction.normalized);
        forwardRotation.x = 0f;
        enemy.transform.rotation = forwardRotation;
    }

    public bool Crouch()
    {
        if (stateType == 0)
        {//屈む
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            enemy.Anim.Play("Crouch");
            if (stateInfo.normalizedTime >= 1.0f)
            {
                stateType = 1;
            }
            return true;
        }
        return false;
    }

    public bool Stand()
    {
        if (stateType == 1)
        {//立った
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            enemy.Anim.Play("Stand");
            if (stateInfo.normalizedTime >= 1.0f)
            {
                stateType = 0;
            }
            return true;
        }
        return false;
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
