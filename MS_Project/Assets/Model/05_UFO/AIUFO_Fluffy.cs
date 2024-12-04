using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUFO_Fluffy : EnemyAction
{
    
    public float floatSpeed = 2.0f; // UFOが目標の高さまで浮き上がる速度
    public float floatPos = 4.0f;   // プレイヤーとのY座標の間隔
    public int floatDilayTime = 100;      //  浮き始める時間
    int floatDilay = 0;

    [SerializeField, Header("突進スピード")]
    public float chargeSpeed = 100.0f;


    bool noGravity = false;
    private Vector3 direction;

    private void Update()
    {
        //距離確認
        distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);

        //攻撃射程外なら浮く
        //if (distanceToPlayer >= EnemyStatus.StatusData.attackDistance)
        //{
        //    noGravity = false;
        //}
        //else
        //{
        //    noGravity = true;
        //    //Debug.Log("get out of my swamp!");
        //}


        //浮いてるかの確認
        if(!noGravity)
        {
            //浮く関数
            Fluffy();
        }
        else
        {
            floatDilay++;
            if(floatDilayTime <= floatDilay)
            {
                noGravity = false;
                floatDilay = 0;
            }
        }

    }

    public override void Chase()
    {
        //ちょっとずつ見る
        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.1f // 補間率（1.0fで即時、0.0fで変化なし）
    );
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
            enemy.Anim.Play("Walk");

            // 追跡
            enemy.OnMovementInput?.Invoke(forwardDirection.normalized);
        }
        else if (distanceToPlayer < enemy.Status.StatusData.attackDistance * 0.5f)
        {
            enemy.Anim.Play("Idle");

            // 後ろ
            enemy.OnMovementInput?.Invoke(-forwardDirection.normalized / 5.0f);
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
        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance && enemy.AllowAttack)
        {
            //クールダウン
            enemy.StartAttackCoroutine();

            stateHandler.TransitionState(ObjectStateType.Attack);
            return;
        }
    }

    public void Attack()
    {
        //ちょっとずつ見る
        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime <= 0.5f)
            enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.04f // 補間率（1.0fで即時、0.0fで変化なし）
    );

        if (noGravity)//浮いてます
        {
            Debug.Log("get out of my swamp!");
            enemy.RigidBody.useGravity = false;
            this.transform.position += transform.forward * 1.0f * Time.deltaTime;
            enemy.RigidBody.MovePosition(this.transform.position);
            //rb.AddForce(transform.up * 10f, ForceMode.Impulse);
        }
        else//浮くのはやめよう
        {
            enemy.RigidBody.useGravity = true;
        }
    }

    // 前にツッコむ
    private void UFOCharge()
    {
        float chargeForce = enemy.RigidBody.mass * chargeSpeed;
        enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);
    }

    //浮きましょう
    public void Fluffy()
    {
        // プレイヤーのY座標 + offsetY を目標地点とする
        Vector3 targetPosition = new Vector3(
            enemy.transform.position.x,
            player.transform.position.y + floatPos,
            enemy.transform.position.z
        );

        // 現在位置から目標位置に向けて移動
        enemy.transform.position = Vector3.Lerp(
            enemy.transform.position,
            targetPosition,
            Time.deltaTime * floatSpeed
        );
    }
    //浮かなくていいです
    public void NotFluffy()
    {
        noGravity = true;
    }

    #region オノマトペ情報
    private void UFOWalkData()
    {
        GenerateWalkOnomatopoeia();
    }

    private void UFOAttackData()
    {
        GenerateAttackOnomatopoeia();
    }
    #endregion
}
