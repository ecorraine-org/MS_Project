using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI; //NavMeshAgentを使うための宣言
using UnityEngine.Playables; //PlayableDirectorを使うための宣言

public class AIGolem : EnemyAction
{
    private Animator anim;
    private AnimatorStateInfo stateInfo;// 現在のアニメーション状態

    //　キャラ状態の定義
    public enum State
    {
        IDLE,
        WALK,
        ATTACK,
        THROW,
        STATEEND
    };
    State stateNo = 0;
    private bool ctrl = true;

    private float maxDistance;
    private float minDistance;

    private float frameTime = 0.0f; //時間計測
    private Vector3 direction;

    protected override void Start()
    {
        base.Start();
        anim = GetComponentInChildren<Animator>();

        maxDistance = enemy.EnemyStatus.StatusData.attackDistance;
        minDistance = enemy.EnemyStatus.StatusData.attackDistance / 1.3f;
    }

    private void Update()
    {
        //距離チェック
        distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);


        // 現在のアニメーション状態を取得
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);


        //射程にいるかどうか
        if (ctrl && distanceToPlayer <= EnemyStatus.StatusData.chaseDistance)
        {

            if (distanceToPlayer <= EnemyStatus.StatusData.attackDistance)
            {
                stateNo = State.ATTACK;
            }
            else if (stateNo == State.IDLE)
            {
                stateNo = State.WALK;
            }
        }
        else
        {
            if (enemy.AllowAttack || enemy.IsAttacking)
            {
                enemy.AllowAttack = false;
                enemy.IsAttacking = false;
            }

            // 停止
            enemy.OnMovementInput?.Invoke(Vector3.zero);
        }


        //デバッグ
        Debug.Log(stateNo);
        
        // 状態ごとの行動
        switch (stateNo)
        {
            case State.IDLE:
                Idle();
                break;
            case State.WALK:
                Walk();
                break;
            case State.ATTACK:
                Attack();
                break;
            case State.THROW:
                Throw();
                break;
        }


        //　敵の向きをプレイヤーの方向に少しづつ変える
        //var dir = (GetDestination() - transform.position).normalized;
        //dir.y = 0;
        //Quaternion setRotation = Quaternion.LookRotation(dir);
        //　算出した方向の角度を敵の角度に設定
        //enemy.transform.rotation = Quaternion.Slerp(transform.rotation, setRotation, navMeshAgent.angularSpeed * 0.1f * Time.deltaTime);
    }

    public override void Chase()
    {
        
    }

    public void Idle()
    {
        stateNo = State.IDLE;
        anim.Play("Idle");
        ctrl = true;
    }

    public void Walk()
    {
        stateNo = State.WALK;
        anim.Play("Walk");//歩こう
        ctrl = true;

        frameTime++; //時間計測

        //しばらく歩いて走りたいかも
        if (frameTime >= 500.0f ||
            distanceToPlayer * 1.5f >= EnemyStatus.StatusData.chaseDistance)
        {
            stateNo = State.THROW;//投げよう
            return;
        }

        if (distanceToPlayer <= EnemyStatus.StatusData.chaseDistance)
        {
            Looking();

            if (distanceToPlayer <= EnemyStatus.StatusData.attackDistance)
            {
                stateNo = State.ATTACK;
                return;
            }
            else
            {
                if (enemy.AllowAttack || enemy.IsAttacking)
                {
                    enemy.AllowAttack = false;
                    enemy.IsAttacking = false;
                }

                // 追跡
                enemy.OnMovementInput?.Invoke(direction.normalized);
            }
        }
        else
        {
            if (enemy.AllowAttack || enemy.IsAttacking)
            {
                enemy.AllowAttack = false;
                enemy.IsAttacking = false;
            }

            // 停止
            enemy.OnMovementInput?.Invoke(Vector3.zero);
        }

    }

    public void Attack()
    {
        stateNo = State.ATTACK;
        anim.Play("Attack");
        ctrl = false;

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.005f // 補間率（1.0fで即時、0.0fで変化なし）
        );

        //アニメーション終了
        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            frameTime = 0.0f;
            stateNo = State.IDLE;
        }
    }

    public void Throw()
    {
        stateNo = State.THROW;
        anim.Play("Throw");
        ctrl = false;

        Looking();

        //アニメーション終了
        if (stateInfo.IsName("Throw") && stateInfo.normalizedTime >= 1.0f)
        {
            frameTime = 0.0f;
            stateNo = State.IDLE;
        }
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

    //　敵キャラの状態を取得するためのメソッド
    public State GetState()
    {
        return stateNo;
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