using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVirus_Tutorial : EnemyAction
{
    private void OnEnable()
    {
        //イベントをバインドする
        TalkManager.OnDialogFinish += DialogFinish;
        PlayerModeManager.OnModelCHange += ReceiveModeCHange;
    }

    private void OnDisable()
    {
        //バインドを解除する
        TalkManager.OnDialogFinish -= DialogFinish;
        PlayerModeManager.OnModelCHange -= ReceiveModeCHange;
    }


    //PlayerController playerComp;

    [SerializeField, Header("突進スピード")]
    public float chargeSpeed = 60.0f;

    private Vector3 direction;

    public float tutorialTimer = 0;

    private void Awake()
    {
    }
    void ReceiveModeCHange(PlayerMode _mode)
    {
        if (enemy.PlayerController.tutorialStage == TutorialStage.Step5)
        {
            Time.timeScale = 0;
            //UI操作
            InputController.Instance.SetInputContext(InputController.InputContext.UI);

            enemy. PlayerController.tutorialStage = TutorialStage.Step6;

            //新しい会話(会話7:変身)
            TalkManager.Instance.LoadNextStory();
            TalkManager.Instance.ShowNextPrefab();
        }
    }
      

void DialogFinish()
    {
        switch (enemy.PlayerController.tutorialStage)
        {
            case TutorialStage.Step1:
                Debug.Log("チュートリアル第1段階");

                //遷移
                enemy.PlayerController.tutorialStage = TutorialStage.Step2;

                //新しい会話(会話3:初めてオノマトペを見た)
                TalkManager.Instance.LoadNextStory();
                TalkManager.Instance.ShowNextPrefab();

                break;
            case TutorialStage.Step2:
                Debug.Log("チュートリアル第2段階");

                //新しい会話(会話4:ペコペコ)
                TalkManager.Instance.LoadNextStory();
                TalkManager.Instance.ShowNextPrefab();

                enemy.PlayerController.tutorialStage = TutorialStage.Step3;


                break;

            case TutorialStage.Step3:
                InputController.Instance.SetInputContext(InputController.InputContext.Player);
                Time.timeScale = 1;
                Debug.Log("チュートリアル第3段階　HP70以下にしてください");

                break;

            case TutorialStage.Step4:
                Debug.Log("敵攻撃時の会話5→チュートリアル第4段階");

                //新しい会話(会話6:)
                TalkManager.Instance.LoadNextStory();
                TalkManager.Instance.ShowNextPrefab();

                enemy.PlayerController.tutorialStage = TutorialStage.Step5;
                break;
            case TutorialStage.Step5:   
                InputController.Instance.SetInputContext(InputController.InputContext.Player);
                Time.timeScale = 1;
                Debug.Log("チュートリアル第5段階　食べてください");
                break;

            //変身、会話7終了
            case TutorialStage.Step6:
             //   Debug.Log("変身の会話7→チュートリアル第6段階");
                InputController.Instance.SetInputContext(InputController.InputContext.Player);
                Time.timeScale = 1;
                Debug.Log("変身の会話7→チュートリアル第6段階 ");
                break;
            //死亡、会話8終了
            case TutorialStage.Step7:
                Debug.Log("美味しかったって会話8→チュートリアル第7段階 ");
                InputController.Instance.SetInputContext(InputController.InputContext.Player);
                Time.timeScale = 1;
                break;
        }
    }

    public void Update()
    {
        //仮でチュートリアル
        switch (enemy.PlayerController.tutorialStage)
        {
            case TutorialStage.None:
                Debug.Log("チュートリアル第1段階");
                if (distanceToPlayer <= 5)
                {
                    tutorialTimer += Time.unscaledDeltaTime;
                    if (tutorialTimer >= 0.7)
                    {
                        tutorialTimer = 0;

                        Debug.Log("チュートリアル第1段階" + enemy.PlayerController.tutorialStage);
                        enemy.PlayerController.tutorialStage = TutorialStage.Step1;
                        //Step初期化
                        TalkManager.Instance.LoadNextStory();
                        TalkManager.Instance.ShowNextPrefab();

                        Time.timeScale = 0;
                        //UI操作
                        InputController.Instance.SetInputContext(InputController.InputContext.UI);

                    }
                }

                break;
            case TutorialStage.Step6:

                if (enemy.Status.CurrentHealth <= 0)
                {
                    enemy.PlayerController.tutorialStage = TutorialStage.Step7;

                    Time.timeScale = 0;
                    //UI操作
                    InputController.Instance.SetInputContext(InputController.InputContext.UI);

             
                    //新しい会話(会話8:変身)
                    TalkManager.Instance.LoadNextStory();
                    TalkManager.Instance.ShowNextPrefab();

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
        if (enemy.PlayerController.tutorialStage > TutorialStage.None)
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
        if (enemy.PlayerController.tutorialStage >= TutorialStage.Step3
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
