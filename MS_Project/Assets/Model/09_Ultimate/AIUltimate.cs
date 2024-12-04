using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI; //NavMeshAgentを使うための宣言
using UnityEngine.Playables; //PlayableDirectorを使うための宣言

public class AIUltimate : EnemyAction
{
    private Animator anim;
    private AnimatorStateInfo stateInfo;// 現在のアニメーション状態

    //　キャラ状態の定義
    public enum State
    {
        IDLE,
        WALK,
        DASH,
        SLASH,
        SLASH_DUAL,
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

        maxDistance = enemy.Status.StatusData.attackDistance;
        minDistance = enemy.Status.StatusData.attackDistance / 1.3f;
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
                stateNo = State.SLASH;
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
            case State.DASH:
                Dash();
                break;
            case State.SLASH:
                Slash();
                break;
            case State.SLASH_DUAL:
                Slash_Dual();
                break;
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
        //Debug.Log(frameTime);
        if (frameTime >= 250.0f ||
            distanceToPlayer * 1.1f >= EnemyStatus.StatusData.chaseDistance)
        {
            stateNo = State.DASH;//走ろう
            return;
        }

        if (distanceToPlayer <= EnemyStatus.StatusData.chaseDistance)
        {
            Looking();

            if (distanceToPlayer <= EnemyStatus.StatusData.attackDistance)
            {
                stateNo = State.SLASH;
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

    public void Dash()
    {
        stateNo = State.DASH;
        ctrl = false;

        if (distanceToPlayer <= EnemyStatus.StatusData.chaseDistance)
        {
            Looking();

            if (distanceToPlayer <= EnemyStatus.StatusData.attackDistance)
            {
                //攻撃
                stateNo = State.SLASH_DUAL;
                return;
            }
            else
            {
                anim.Play("Dash");//走ろう
                if (enemy.AllowAttack || enemy.IsAttacking)
                {
                    enemy.AllowAttack = false;
                    enemy.IsAttacking = false;
                }

                // 追跡
                enemy.OnMovementInput?.Invoke(direction.normalized * 7);
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

    public void Slash()
    {
        stateNo = State.SLASH;
        anim.Play("Slash");
        ctrl = false;

        // 停止
        enemy.OnMovementInput?.Invoke(Vector3.zero);
        //アニメーション停止
        if (stateInfo.IsName("Slash") && stateInfo.normalizedTime >= 1.0f)
        {
            frameTime = 0.0f;
            stateNo = State.IDLE;
            Debug.Log("アニメーション 'Slash' が終了しました！");
        }
        //enemy.OnMovementInput?.Invoke(Vector3.zero);
    }

    public void Slash_Dual()
    {
        stateNo = State.SLASH_DUAL;
        anim.Play("Slash_Dual");
        ctrl = false;

        // 停止
        enemy.OnMovementInput?.Invoke(Vector3.zero);

        //アニメーション終了
        if (stateInfo.IsName("Slash_Dual") && stateInfo.normalizedTime >= 1.0f)
        {
            frameTime = 0.0f;
            stateNo = State.IDLE;
            Debug.Log("アニメーション 'Slash_Dual' が終了しました！");
        }
        //enemy.OnMovementInput?.Invoke(Vector3.zero);
    }

    //　タイムラインで状態をStateEnd状態に設定するためのメソッド
    public void EndState()
    {
        SetState(State.STATEEND); ;
    }

    //　敵キャラの状態を設定するためのメソッド 
    public void SetState(State tempState, Transform targetObject = null)
    {
        stateNo = tempState;

        if (tempState == State.IDLE)
        {
            //navMeshAgent.isStopped = true; //キャラの移動を止める
            //animator.SetBool("chase", false); //アニメーションコントローラーのフラグ切替（Chase⇒Idle）
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
