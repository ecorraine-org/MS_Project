using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParadinActionList : EnemyAction
{
    [SerializeField, Header("ターゲットレイヤー")]
    LayerMask targetLayer;

    //時間計測
    float frameTime = 0.0f;

    //移動を表す　0:歩く 1:走る
    int moveStage = 0;

    private Vector3 direction;

    [SerializeField, Header("投げるプレハブ")]
    private GameObject projectilePrefab;
    [SerializeField, Header("プレハブの生成位置")]
    private Transform spawnPoint;
    [SerializeField, Header("VFXプレハブ")]
    private GameObject explosionPrefab;



    #region Died
    public void DiedInit()
    {
        enemy.Anim.Play("Died", 0, 0.0f);
    }
    public void DiedTick()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //後ろへ
        if (stateInfo.normalizedTime < 0.5f)
        {
            direction = player.position - enemy.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            targetRotation.x = 0f;
            targetRotation.z = 0f;

            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.1f // 補間率（1.0fで即時、0.0fで変化なし）
        );

            Vector3 forceDirection = -enemy.transform.forward * 0.6f; // 後ろ方向の力
            enemy.GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.VelocityChange);
        }


    }
    #endregion

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

        //ダメージチェック
        //if (stateHandler.CheckHit()) return;

        //移動へ遷移
        float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);

        if (distanceToPlayer > enemyStatus.StatusData.attackDistance * 0.7f &&
            distanceToPlayer <= enemyStatus.StatusData.chaseDistance)
        {
            enemy.State.TransitionState(ObjectStateType.Walk);
            return;
        }

        //リストへ遷移
        if (CheckListTimer())
        {
            enemy.State.TransitionState(ObjectStateType.Skill);
            return;
        }

    }
    #endregion

    #region Walk

    /// <summary>
    /// 移動処理初期化(一回だけ実行する)
    /// </summary>
    public void WalkInit()
    {
        //初期化
        frameTime = 0.0f;

        enemy.Anim.Play("Walk");


    }

    public void WalkTick()
    {
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime; //時間計測

        HandleWalk();

        //移動
        enemy.Move();

    }

    //WalkTickに呼び出される
    private void HandleWalk()
    {
        // 追跡
        enemy.OnMovementInput?.Invoke(direction.normalized);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.03f // 補間率（1.0fで即時、0.0fで変化なし）
    );


        if (frameTime >= 2.5f &&
              distanceToPlayer >= EnemyStatus.StatusData.attackDistance * 2.5f)
        {
            //移動状態(走り)へ遷移
            stateHandler.TransitionState(ObjectStateType.Skill);//リストへ遷移

            return;
        }

        //攻撃へ遷移
        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance && CheckListTimer())
        {
            stateHandler.TransitionState(ObjectStateType.Skill);//リストへ遷移
            return;
        }

        //近いよ
        if (distanceToPlayer < enemyStatus.StatusData.attackDistance * 0.7f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
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

        //歩き
        enemy.Anim.Play("Walk");

        currentUpdateAction = MoveTick;
    }

    public void MoveTick()
    {
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        HandleWalk();

        //移動
        enemy.Move();

    }
    #endregion


    #region Attack

    /// <note>
    /// 関数名は「Attack」や「Skill」にならないように
    /// </note>
    public void AttackInit()
    {

        animator.Play("Attack");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = AttackTick;
    }

    public void AttackTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        if (stateHandler.CheckDeath()) return;

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);


        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        if (stateInfo.normalizedTime <= 0.1f)
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.01f // 補間率（1.0fで即時、0.0fで変化なし）
        );

        //アニメーション終了
        if (stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Rush

    public void RushInit()
    {

        animator.Play("Rush");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = RushTick;

    }

    public void RushTick()
    {
        //攻撃判定
        enemy.AttackCollider.DetectColliders(10.0f, false);

        //移動
        enemy.Move();

        //死んでいるかと時間計測
        if (stateHandler.CheckDeath()) return;
        frameTime += Time.deltaTime;

        // 前に進行
        float chargeForce = enemy.RigidBody.mass * 1.5f;
        enemy.RigidBody.AddForce(enemy.transform.forward * chargeForce, ForceMode.Impulse);


        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        enemy.transform.rotation = Quaternion.Slerp(
    enemy.transform.rotation,
    targetRotation,
    0.01f // 補間率（1.0fで即時、0.0fで変化なし）
);

        //アニメーション終了
        if (stateInfo.IsName("Rush") && frameTime >= 2.8f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion

    #region Throw

    public void ThrowInit()
    {

        animator.Play("Throw");

        frameTime = 0;

        //Updateで呼び出すために必須のバインド
        //呼び出したい関数に変更する
        currentUpdateAction = ThrowTick;
    }

    public void ThrowTick()
    {
        if (stateHandler.CheckDeath()) return;

        //アニメーションイベントで設定する必要ある(EnableHit DisableHit)
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, targetLayer, false);


        //攻撃判定
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        if (stateInfo.normalizedTime <= 0.3f)
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            0.01f // 補間率（1.0fで即時、0.0fで変化なし）
        );

        //アニメーション終了
        if (stateInfo.normalizedTime >= 1.0f)
        {
            stateHandler.TransitionState(ObjectStateType.Idle);
        }
    }
    #endregion


    #region ActionList

    /// <summary>
    /// 初期化
    /// </summary>
    public void SkillInit()
    {

        actionPatternList[listIndex].GetActionPattern()[actionStage].OnAction?.Invoke();


    }

    public void SkillTick()
    {
        //バインドした関数を呼び出す
        currentUpdateAction?.Invoke();
    }

    #endregion


    // 斜方投射
    private void ThrowHammer()
    {
        // プレハブのインスタンスを生成
        GameObject thrownHammer = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation, collector.transform);
        Rigidbody rbHammer = thrownHammer.GetComponent<Rigidbody>();
        rbHammer.useGravity = false; // 円運動中は重力を無効化

        collector.GetComponent<ObjectCollector>().otherObjectPool.Add(thrownHammer);

        // 円運動の設定
        float radius = 8.0f; // 円の半径
        float speed = 3.0f;  // 回転速度

        // 初期位置を計算
        float angle = 0.0f; // 初期角度
        Vector3 circularDirection = new Vector3(
            Mathf.Cos(angle) * radius, // X座標 (円のX方向)
            0.0f,                      // 高さ（水平）
            Mathf.Sin(angle) * radius  // Z座標 (円のZ方向)
        );

        // 円運動を計算する力
        Vector3 force = speed * circularDirection.normalized;
        rbHammer.AddForce(force, ForceMode.Impulse);

        // 衝突時に削除するコンポーネントを追加
        thrownHammer.AddComponent<DestroyOnCollision>();

        // 指定時間後に削除
        Destroy(thrownHammer, 2.0f);

        // 円運動の開始
        StartCoroutine(CircularMotion(thrownHammer, rbHammer, 18.0f, 1.0f));
    }

    // 新しいクラス: 衝突時に卵を削除する
    public class DestroyOnCollision : MonoBehaviour
    {
        private ParadinActionList paradinHammer;

        private void Start()
        {
            paradinHammer = FindFirstObjectByType<ParadinActionList>(); // ParadinActionListのインスタンスを取得
        }

        private void OnCollisionEnter(Collision collision)
        {
            // プレイヤーと衝突した場合に卵を削除
            if (collision.gameObject.CompareTag("Player"))
            {
                if (paradinHammer != null)
                {
                    Instantiate(paradinHammer.explosionPrefab, transform.position, Quaternion.identity, paradinHammer.collector.transform);
                }
                Destroy(gameObject);
            }
            // プレイヤー以外と衝突した場合にも卵を削除
            else if (!collision.gameObject.CompareTag("Player"))
            {
                if (paradinHammer != null)
                {
                    Instantiate(paradinHammer.explosionPrefab, transform.position, Quaternion.identity, paradinHammer.collector.transform);
                }
                Destroy(gameObject);
            }
        }
    }


    // 円運動を作るコルーチン
    private IEnumerator CircularMotion(GameObject cutter, Rigidbody rb, float speed, float moveTime)
    {
        float elapsedTime = 0f; // 経過時間を計測
        Vector3 direction = cutter.transform.forward; // 初期は前進

        while (true)
        {
            // Y軸を中心に高速回転
            cutter.transform.Rotate(0, 1500.0f * Time.deltaTime, 0, Space.Self);

            // 移動
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

            // 経過時間を更新
            elapsedTime += Time.deltaTime;

            // moveTimeを超えたら進行方向を反転
            if (elapsedTime >= moveTime)
            {
                elapsedTime = 0f; // 経過時間をリセット
                direction = -direction; // 進行方向を反転
            }

            yield return null; // 次のフレームまで待機
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

  

    #region オノマトペ生成情報
    private void ParadinWalkData()
    {
        GenerateWalkOnomatopoeia();
    }

    private void ParadinAttackData()
    {
        GenerateAttackOnomatopoeia();
    }
    #endregion
}
