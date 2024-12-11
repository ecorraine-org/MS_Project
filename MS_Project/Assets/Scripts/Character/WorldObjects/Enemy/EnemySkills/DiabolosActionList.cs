using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiabolosActionList : EnemyAction
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

        //リストへ遷移
        if (CheckListTimer())
        {
            enemy.State.TransitionState(ObjectStateType.Skill);
            return;
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

    #region Punch
    /// <summary>
    /// 投げ初期化
    /// </summary>
    public void PunchInit()
    {
        animator.Play("Punch");

        frameTime = 0.0f;

        currentUpdateAction = PunchTick;
    }

    public void PunchTick()
    {
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;


        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //パンチ終了
        if (stateInfo.IsName("Punch"))
        {
            if (frameTime >= 4.0f)
                animator.Play("Punch_End");
        }

        //ニュートラルに戻る
        if (stateInfo.IsName("Punch_End") &&
                stateInfo.normalizedTime >= 1.0f)
            stateHandler.TransitionState(ObjectStateType.Idle);
    }
    #endregion

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
