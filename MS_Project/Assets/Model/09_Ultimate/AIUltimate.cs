using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI; //NavMeshAgentを使うための宣言
using UnityEngine.Playables; //PlayableDirectorを使うための宣言

public class AIUltimate : EnemyAction
{


    [SerializeField, Header("突進スピード")]
    public float chargeSpeed = 60.0f;
    private Animator anim;

    //　キャラ状態の定義
    public enum StateNo
    {
        Idle,
        Chase,
        Attack,
        StateEnd
    };
    StateNo state = 0;

    private float maxDistance;
    private float minDistance;

    float frameTime = 0.0f; //時間計測


    protected override void Start()
    {
        base.Start();

        maxDistance = enemy.EnemyStatus.StatusData.attackDistance;
        minDistance = enemy.EnemyStatus.StatusData.attackDistance / 1.3f;
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);


        if (distanceToPlayer < minDistance)
        {
            

        }
        else
        {
            anim.Play("Damaged");
        }

        //　敵の向きをプレイヤーの方向に少しづつ変える
        //var dir = (GetDestination() - transform.position).normalized;
        //dir.y = 0;
        //Quaternion setRotation = Quaternion.LookRotation(dir);
        //　算出した方向の角度を敵の角度に設定
        //transform.rotation = Quaternion.Slerp(transform.rotation, setRotation, navMeshAgent.angularSpeed * 0.1f * Time.deltaTime);
    }

    public override void Chase()
    {
        frameTime += Time.deltaTime; //時間計測
        Debug.Log(frameTime);
        if (frameTime >= 10.0f)
        {
            Dash();
        }

        if (distanceToPlayer <= EnemyStatus.StatusData.chaseDistance)
        {
            Vector3 direction = player.position - enemy.transform.position;
            // 進む方向に向く
            Quaternion forwardRotation = Quaternion.LookRotation(direction.normalized);
            forwardRotation.x = 0f;
            enemy.transform.rotation = forwardRotation;

            if (distanceToPlayer <= EnemyStatus.StatusData.attackDistance)
            {
                enemy.AllowAttack = true;
                enemy.OnMovementInput?.Invoke(Vector3.zero);

                // 攻撃
                enemy.OnAttack?.Invoke();
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

    public void Dash()
    {
        if (distanceToPlayer <= EnemyStatus.StatusData.chaseDistance)
        {
            Vector3 direction = player.position - enemy.transform.position;
            // 進む方向に向く
            Quaternion forwardRotation = Quaternion.LookRotation(direction.normalized);
            forwardRotation.x = 0f;
            enemy.transform.rotation = forwardRotation;

            if (distanceToPlayer <= EnemyStatus.StatusData.attackDistance)
            {
                enemy.AllowAttack = true;
                enemy.OnMovementInput?.Invoke(Vector3.zero);

                // 攻撃
                enemy.OnAttack?.Invoke();
            }
            else
            {
                if (enemy.AllowAttack || enemy.IsAttacking)
                {
                    enemy.AllowAttack = false;
                    enemy.IsAttacking = false;
                }

                // 追跡
                enemy.OnMovementInput?.Invoke(direction.normalized * 5);
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

    //　タイムラインで状態をStateEnd状態に設定するためのメソッド
    public void EndState()
    {
        SetState(StateNo.StateEnd); ;
    }

    //　敵キャラの状態を設定するためのメソッド 
    public void SetState(StateNo tempState, Transform targetObject = null)
    {
        state = tempState;

        if (tempState == StateNo.Idle)
        {
            //navMeshAgent.isStopped = true; //キャラの移動を止める
            //animator.SetBool("chase", false); //アニメーションコントローラーのフラグ切替（Chase⇒Idle）
        }
    }

    //　敵キャラの状態を取得するためのメソッド
    public StateNo GetState()
    {
        return state;
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
