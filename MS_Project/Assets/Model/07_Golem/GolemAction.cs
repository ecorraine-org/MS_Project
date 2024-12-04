using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI; //NavMeshAgentを使うための宣言
using UnityEngine.Playables; //PlayableDirectorを使うための宣言

public class GolemAction : EnemyAction
{
    [SerializeField, Header("ターゲットレイヤー")]
    LayerMask targetLayer;

    //時間計測
    float frameTime = 0.0f;
    //float ctrlDilay = 0.0f;   //動くまでの時間

    private Vector3 direction;
    private float frontBackDistance = 0.0f;//前後の距離
    private float leftRightDistance = 0.0f;//左右の距離

    private void Update()
    {
        //ctrlDilay++;

        //if (ctrlDilay <= 60.0f) return;

        //距離チェック
        distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);
        Vector3 directionToPlayer = (player.position - enemy.transform.position).normalized;

        // enemyの前方向と右方向ベクトルを取得
        Vector3 forward = enemy.transform.forward; // 前方向
        Vector3 right = enemy.transform.right;     // 右方向

        // 前後と左右の距離をDotで計算
        frontBackDistance = Vector3.Dot(forward, directionToPlayer); // 前後の判定
        leftRightDistance = Vector3.Dot(right, directionToPlayer);   // 左右の判定

        //Debug.Log(stateHandler.CurrentStateType);

        // frontBackDistance > 0.0f: プレイヤーは前方にいる
        // frontBackDistance < 0.0f: プレイヤーは後方にいる
        // leftRightDistance > 0.0f: プレイヤーは右側にいる
        // leftRightDistance < 0.0f: プレイヤーは左側にいる
    }

    #region Walk

    /// <summary>
    /// 移動処理初期化(一回だけ実行する)
    /// </summary>
    public void WalkInit()
    {
        //Debug.Log("WalkInit");

        //初期化
        frameTime = 0.0f;

        //歩き
        enemy.Anim.Play("Walk");
        //走り
        //if (moveStage == 1) enemy.Anim.Play("Dash");

    }

    public void WalkTick()
    {
        frameTime++; //時間計測

        //横攻撃
        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance * 1.2f && enemy.AllowAttack &&
            (leftRightDistance <= -0.7f || leftRightDistance >= 0.7f))
        {
            //スキル状態へ遷移
            //stateHandler.TransitionState(ObjectStateType.Skill);
            //return;
        }


        //歩くのか走るのか
        HandleWalk();

        //じわりとみる
        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.01f // 補間率（1.0fで即時、0.0fで変化なし）
    );

        //移動
        enemy.Move();
        
    }

    //WalkTickに呼び出される
    private void HandleWalk()
    {
        // 追跡
        enemy.OnMovementInput?.Invoke(direction.normalized);

        //遠いと投げる
        if ((frameTime >= 180.0f &&
              distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 2.0f) ||
              distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 5.0f)
        {
            //投げよう
            stateHandler.TransitionState(ObjectStateType.Skill);
            return;
        }

        //攻撃へ遷移
        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance)
        {
            //クールダウン
            enemy.StartAttackCoroutine();

            stateHandler.TransitionState(ObjectStateType.Attack);
            return;
        }
    }

    #endregion

    #region Attack

    public void AttackInit()
    {
        animator.Play("Attack");
        //Debug.Log("左右の距離: " + leftRightDistance);
        frameTime = 0;
    }

    public void AttackTick()
    {
        // 追跡
        enemy.OnMovementInput?.Invoke(direction.normalized * 0.5f);

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime <= 0.3f)
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.03f // 補間率（1.0fで即時、0.0fで変化なし）
        );

        //アニメーション終了
        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
            //ctrlDilay = 0.0f;
        }
    }
    #endregion

    #region Throw
    /// <summary>
    /// 投げ初期化
    /// </summary>
    public void SkillInit()
    {
        animator.Play("Throw");
        frameTime = 0.0f;
    }

    public void SkillTick()
    {
        //ちょっとずつ見る
        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Throw") && stateInfo.normalizedTime <= 0.4f)
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.01f // 補間率（1.0fで即時、0.0fで変化なし）
        );

        //アニメーション終了
        if (stateInfo.IsName("Throw") && stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
            //ctrlDilay = 0.0f;
        }
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
