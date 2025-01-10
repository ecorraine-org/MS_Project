using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVirus_Tutorial : EnemyAction
{
    private void OnEnable()
    {
        //イベントをバインドする
        TalkManager.OnDialogueFinish += DialogFinish;
        PlayerModeManager.OnModeChange += ReceiveModeChange;
    }

    private void OnDisable()
    {
        //バインドを解除する
        TalkManager.OnDialogueFinish -= DialogFinish;
        PlayerModeManager.OnModeChange -= ReceiveModeChange;
    }

    //PlayerController playerComp;

    [SerializeField, Header("突進スピード")]
    public float chargeSpeed = 60.0f;

    private Vector3 direction;

    public float tutorialTimer = 0;

    /// <summary>
    /// チュートリアル段階を変更
    /// </summary>
    /// <param name="_phase">チュートリアル段階</param>
    /// <param name="index">会話段階</param>
    void ChangeTutorialPhase(int _dialogueIndex, TutorialPhase _phase)
    {
        Time.timeScale = 0;
        //UI操作に変換
        InputController.Instance.SetInputContext(InputController.InputContext.UI);

        //新しい会話を読み込み
        TalkManager.Instance.LoadStory(_dialogueIndex);

        enemy.PlayerController.tutorialStage = _phase;
    }

    void ReceiveModeChange(PlayerMode _mode)
    {
        Debug.Log("食べたイベント");
        if (enemy.PlayerController.tutorialStage == TutorialPhase.Phase5)
        {
            CustomLogger.Log("チュートリアル第５段階：「捕食による変身」終了");
            //会話３：「変身」
            ChangeTutorialPhase(3, TutorialPhase.Phase6);
        }
    }

    public override void TutorialStopTime()
    {
        if (enemy.PlayerController.tutorialStage == TutorialPhase.Phase3)
        {
            CustomLogger.Log("チュートリアル第３段階：「敵を弱める」終了\nオノマトペ案内の準備");

            //会話２：「オノマトペの捕食」
            ChangeTutorialPhase(2, TutorialPhase.Phase4);
        }
    }

    void DialogFinish()
    {
        Debug.Log("受信時段階: " + enemy.PlayerController.tutorialStage);
        switch (enemy.PlayerController.tutorialStage)
        {
            case TutorialPhase.Phase1:
                /*
                //新しい会話(会話3:初めてオノマトペを見た)
                TalkManager.Instance.LoadStory(2);
                */
                break;

            case TutorialPhase.Phase2:
                CustomLogger.Log("チュートリアル第２段階：「敵と出会った」終了\n敵のHPを70までに弱める");
                InputController.Instance.SetInputContext(InputController.InputContext.Player);
                Time.timeScale = 1;

                enemy.PlayerController.tutorialStage = TutorialPhase.Phase3;
                /*
                Debug.Log("チュートリアル第2段階");

                //新しい会話(会話4: ペコペコ)
                TalkManager.Instance.LoadStory(3);
                */
                break;

            case TutorialPhase.Phase3:
                /*
                InputController.Instance.SetInputContext(InputController.InputContext.Player);
                Time.timeScale = 1;
                Debug.Log("チュートリアル第3段階　HP70以下にしてください");
                */
                break;

            case TutorialPhase.Phase4:
                CustomLogger.Log("チュートリアル第４段階：「オノマトペ案内」終了\n捕食による変身の準備");
                InputController.Instance.SetInputContext(InputController.InputContext.Player);
                Time.timeScale = 1;

                /*
                //新しい会話(会話6:)
                TalkManager.Instance.LoadStory(5);
                */
                enemy.PlayerController.tutorialStage = TutorialPhase.Phase5;
                break;

                //捕食による変身
            case TutorialPhase.Phase5:
                /*
                InputController.Instance.SetInputContext(InputController.InputContext.Player);
                Time.timeScale = 1;
                Debug.Log("チュートリアル第5段階　食べてください");
                */
                break;

            //変身
            case TutorialPhase.Phase6:
                CustomLogger.Log("チュートリアル第６段階：「捕食による変身」終了");
                InputController.Instance.SetInputContext(InputController.InputContext.Player);
                Time.timeScale = 1;
                break;

            //終了
            case TutorialPhase.Phase7:
                CustomLogger.Log("チュートリアル第７段階：「どんどん行くぞ」終了");
                InputController.Instance.SetInputContext(InputController.InputContext.Player);
                Time.timeScale = 1;

                enemy.PlayerController.tutorialStage = TutorialPhase.TutorialEnd;
                break;
        }
    }

    public void Update()
    {
        //仮でチュートリアル
        switch (enemy.PlayerController.tutorialStage)
        {
            case TutorialPhase.None:
                enemy.PlayerController.tutorialStage = TutorialPhase.Phase1;
                break;

            case TutorialPhase.Phase1:
                //Debug.Log("チュートリアル第0段階");
                if (distanceToPlayer <= 5)
                {
                    tutorialTimer += Time.unscaledDeltaTime;
                    if (tutorialTimer >= 0.5)
                    {
                        tutorialTimer = 0;

                        CustomLogger.Log("チュートリアル第１段階距離内" + enemy.PlayerController.tutorialStage);

                        //Step初期化、会話1：「別の生き物と出会い」
                        ChangeTutorialPhase(1, TutorialPhase.Phase2);
                    }
                }
                break;

            case TutorialPhase.Phase6:
                if (enemy.Status.CurrentHealth <= 0)
                {
                    //新しい段階に移行、会話4：「美味しかった？」
                    ChangeTutorialPhase(4, TutorialPhase.Phase7);
                }
                break;
        }
    }

    public void AttackTick()
    {
        // Debug.Log("tuto AttackTick更新している");
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        if (stateHandler.CheckDeath()) return;

        if (enemy.AnimManager != null && enemy.AnimManager.IsAnimEnd)
        {
            enemy.State.TransitionState(ObjectStateType.Idle);
        }
    }

    // 前にツッコむ
    private void SlimeCharge()
    {
        float chargeForce = enemy.RigidBody.mass * chargeSpeed;
        enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);
    }

    public void WalkInit()
    {
        enemy.Anim.Play("Walk");
    }

    public void WalkTick()
    {
        if (enemy.PlayerController.tutorialStage > TutorialPhase.None)
        {
            //ダメージチェック
            if (stateHandler.CheckHit()) return;
        }

        enemy.Move();

        // enemyの現在の角度を基にした前方向
        Vector3 forwardDirection = enemy.transform.forward;
        forwardDirection.y = 0f;// 地面に沿った移動

        //適度に距離を置く
        distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);

        if (distanceToPlayer >= enemy.Status.StatusData.attackDistance)
        {
            // 追跡
            enemy.OnMovementInput?.Invoke(forwardDirection.normalized);
        }
        else if (distanceToPlayer < enemy.Status.StatusData.attackDistance * 0.5f)
        {
            // 後ろ
            enemy.OnMovementInput?.Invoke(-forwardDirection.normalized / 2.0f);
        }

        //ちょっとずつ見る
        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.1f // 補間率（1.0fで即時、0.0fで変化なし）
    );

        //攻撃へ遷移
        if (enemy.PlayerController.tutorialStage >= TutorialPhase.Phase3
            && enemy.Status.CurrentHealth <= 0.8 * enemy.Status.StatusData.maxHealth
            && distanceToPlayer <= enemyStatus.StatusData.attackDistance && enemy.AllowAttack)
        {
            //クールダウン
            enemy.StartAttackCoroutine();

            stateHandler.TransitionState(ObjectStateType.Attack);
            return;
        }
    }

    #region オノマトペ情報
    private void VirusWalkData()
    {
        GenerateWalkOnomatopoeia();
    }

    private void VirusAttackData()
    {
        GenerateAttackOnomatopoeia();
    }
    #endregion
}
