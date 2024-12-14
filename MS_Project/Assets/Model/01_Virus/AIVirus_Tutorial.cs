using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVirus_Tutorial : EnemyAction
{

    //PlayerController playerComp;

    [SerializeField, Header("突進スピード")]
    public float chargeSpeed = 60.0f;

    private Vector3 direction;

    private void Awake()
    {
      //  playerComp = player.GetComponent<PlayerController>();
    }

    public void Update()
    {
      

        //if (playerComp == null)
        //{
        //    playerComp = player.GetComponent<PlayerController>();
        //}


        distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);

      //  Debug.Log("distanceToPlayer "+ distanceToPlayer);
        if (distanceToPlayer <= 5 && enemy.PlayerController.tutorialStage == TutorialStage.None)
        {

            enemy.PlayerController.tutorialStage = TutorialStage.Step1;

            Time.timeScale = 0;
            //UI操作
            InputController.Instance.SetInputContext(InputController.InputContext.UI);
        }

        //HP70以下攻撃
        if (enemy.PlayerController.tutorialStage == TutorialStage.Step2&&enemy.Status.CurrentHealth<=0.7* enemy.Status.StatusData.maxHealth)
        {
            stateHandler.TransitionState(ObjectStateType.Attack);

        }
    }

    
    private void AttackTick()
    {

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
        //ダメージチェック
        if (stateHandler.CheckHit()) return;

        enemy.Move();

        // enemyの現在の角度を基にした前方向
        Vector3 forwardDirection = enemy.transform.forward;
        forwardDirection.y = 0f;// 地面に沿った移動

        //適度に距離を置く
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
        //if (distanceToPlayer <= enemyStatus.StatusData.attackDistance && enemy.AllowAttack)
        //{
        //    //クールダウン
        //    enemy.StartAttackCoroutine();

        //    stateHandler.TransitionState(ObjectStateType.Attack);
        //    return;
        //}
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
