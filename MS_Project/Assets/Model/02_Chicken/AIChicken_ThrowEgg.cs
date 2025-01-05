using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AIChicken_ThrowEgg : EnemyAction
{
    [SerializeField, Header("投げるプレハブ")]
    private GameObject projectilePrefab;
    [SerializeField, Header("プレハブの生成位置")]
    private Transform spawnPoint;
    [SerializeField, Header("投擲角度（上向き）")]
    float upAngle = 70f;
    [SerializeField, Header("投擲角度（プレイヤー向き）")]
    float playerAngle = 70f;

    [SerializeField, Header("投げる力")]
    float throwForce = 10f;

    [SerializeField, Header("VFXプレハブ")]
    private GameObject explosionPrefab;

    /*
    [SerializeField, Header("連続して投げる回数")]
    int throwCount = 3;
    [SerializeField, Header("投擲の間隔（秒）")]
    float throwInterval = 0.5f;
    [Tooltip("現在の投擲回数")]
    private int currentThrow = 0;
    */

    [Tooltip("投擲中フラグ")]
    private bool isThrowing = false;

    private float maxDistance;
    private float minDistance;

    private Vector3 direction;


    protected override void Start()
    {
        base.Start();

        maxDistance = enemy.Status.StatusData.attackDistance;
        minDistance = enemy.Status.StatusData.attackDistance / 1.3f;
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

        //じわりとみる準備
        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        if (distanceToPlayer < minDistance)
            {//逃げよう

            //ジワリと見る
            enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.1f // 補間率（1.0fで即時、0.0fで変化なし）
    );
            Debug.Log("back");
            enemy.Anim.Play("Escape");
            enemy.OnMovementInput?.Invoke(-direction.normalized / 2);
            }
            else if (distanceToPlayer <= maxDistance && distanceToPlayer > minDistance)
            {//攻撃しましょう
            Debug.Log("attack");

            enemy.Anim.Play("Idle");
            enemy.OnMovementInput?.Invoke(Vector3.zero);

            if (!enemy.AllowAttack) return;

            //クールダウン
            enemy.StartAttackCoroutine();

                stateHandler.TransitionState(ObjectStateType.Attack);
                return;
            }
            else if (distanceToPlayer > maxDistance)
            {//追跡しよう

            Debug.Log("walk");

            enemy.Anim.Play("Walk");

            //ジワリと見る
            enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.2f // 補間率（1.0fで即時、0.0fで変化なし）
    );

            // 自分を基準にした前進
            Vector3 forwardDirection = enemy.transform.forward;
                forwardDirection.y = 0f;// 地面に沿った移動
                // 追跡
                enemy.OnMovementInput?.Invoke(forwardDirection.normalized);


            if (distanceToPlayer <= EnemyStatus.StatusData.chaseDistance)
            {

                //enemy.OnMovementInput?.Invoke(Vector3.zero);
            }
        }
    }

    public void AttackInit()
    {
        enemy.Anim.SetTrigger("IsAttack");
    }

    public void AttackTick()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        stateHandler = enemy.State;
        if (stateHandler.CheckDeath()) return;

        //ダメージチェック
        if (stateHandler.CheckHit()) return;

        //じわりとみる
        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.1f // 補間率（1.0fで即時、0.0fで変化なし）
    );
        //アイドルへ遷移
        /*
        if (objController.MovementInput.magnitude <= 0f && !objController.IsAttacking)
            enemyStateHandler.TransitionState(ObjectStateType.Idle);
        */
        if (stateInfo.IsName("PostSkill") && stateInfo.normalizedTime >= 3.0f)
        {
            enemy.State.TransitionState(ObjectStateType.Idle);
        }
    }

    // アニメーションイベントから呼び出すメソッド
    public void BeginThrowSequence()
    {
        if (!isThrowing)
        {
            //currentThrow = 0;
            isThrowing = true;
            ThrowEgg();
            //InvokeRepeating(nameof(ThrowEgg), 0f, throwInterval);
        }
    }

    // アニメーションイベントから呼び出すメソッド
    public void BeginCutterSequence()
    {
        if (!isThrowing)
        {
            //currentThrow = 0;
            isThrowing = true;
            ThrowCutter();
            //InvokeRepeating(nameof(ThrowEgg), 0f, throwInterval);
        }
    }

    //--------------------------------------------------------------
    // 斜方投射
    private void ThrowEgg()
    {
        // プレハブのインスタンスを生成
        GameObject thrownEgg = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation, collector.transform);
        Rigidbody rbEgg = thrownEgg.GetComponent<Rigidbody>();
        rbEgg.useGravity = true;

        collector.GetComponent<ObjectCollector>().otherObjectPool.Add(thrownEgg);

        Quaternion rotateToPlayer = Quaternion.LookRotation((player.position - spawnPoint.position).normalized);
        spawnPoint.rotation = rotateToPlayer;

        // 前に飛ばす準備
        Vector3 forwardDirection = enemy.transform.forward;
        forwardDirection.y = 0f;// 地面に水平に

        float radianPlayerAngle = playerAngle * Mathf.Deg2Rad;
        float radianUpAngle = upAngle * Mathf.Deg2Rad;
        // 指定した角度に飛ばす
        Vector3 playerDirection = spawnPoint.forward /*+ (Vector3.forward * Mathf.Cos(radianPlayerAngle))*/;
        // 前方に対してn度上向きに飛ばす
        Vector3 upDirection = spawnPoint.up * Mathf.Sin(radianUpAngle);
        // 指定角度に応じた方向を計算(自分から見て前に投げちゃう)
        Vector3 forceDirection = forwardDirection.normalized;

        // 力を計算して加える
        Vector3 force = throwForce * forceDirection;
        rbEgg.AddForce(force, ForceMode.Impulse);

        float gravityScale = 1.0f; // 重力を通常より強くする
                                   // 重力の強化（重力を上乗せするために下方向の追加力を適用）
        Vector3 additionalGravity = Physics.gravity * (gravityScale - 1.0f); // gravityScaleが1なら通常重力、2なら2倍
        rbEgg.AddForce(additionalGravity, ForceMode.Acceleration);

        // 回転を加える
        Vector3 torque = new Vector3(400.0f, spawnPoint.forward.y, spawnPoint.forward.z); // Z軸を中心に回転するトルク
        rbEgg.AddTorque(torque, ForceMode.Impulse);

        // 衝突時に卵を削除するための処理
        thrownEgg.AddComponent<DestroyOnCollision>();

        // 5秒後に卵オブジェクトを削除
        Destroy(thrownEgg, 10f);

        isThrowing = false;
    }

    // 新しいクラス: 衝突時に卵を削除する
    public class DestroyOnCollision : MonoBehaviour
    {
        private AIChicken_ThrowEgg aiChickenThrowEgg;

        private void Start()
        {
            aiChickenThrowEgg = FindFirstObjectByType<AIChicken_ThrowEgg>(); // AIChicken_ThrowEggのインスタンスを取得
        }

        private void OnCollisionEnter(Collision collision)
        {
            // プレイヤーと衝突した場合に卵を削除
            if (collision.gameObject.CompareTag("Player"))
            {
                if (aiChickenThrowEgg != null)
                {
                    Instantiate(aiChickenThrowEgg.explosionPrefab, transform.position, Quaternion.identity, aiChickenThrowEgg.collector.transform);
                }
                Destroy(gameObject);
            }
            // プレイヤー以外と衝突した場合にも卵を削除
            else if (!collision.gameObject.CompareTag("Player"))
            {
                if (aiChickenThrowEgg != null)
                {
                    Instantiate(aiChickenThrowEgg.explosionPrefab, transform.position, Quaternion.identity, aiChickenThrowEgg.collector.transform);
                }
                Destroy(gameObject);
            }
        }
    }

    // 斜方投射
    private void ThrowCutter()
    {
        // プレハブのインスタンスを生成
        GameObject thrownCutter = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation, collector.transform);
        Rigidbody rbEgg = thrownCutter.GetComponent<Rigidbody>();
        rbEgg.useGravity = false; // 円運動中は重力を無効化

        collector.GetComponent<ObjectCollector>().otherObjectPool.Add(thrownCutter);

        // 円運動の設定
        float radius = 8.0f; // 円の半径
        float speed = 2.0f;  // 回転速度

        // 初期位置を計算
        float angle = 0.0f; // 初期角度
        Vector3 circularDirection = new Vector3(
            Mathf.Cos(angle) * radius, // X座標 (円のX方向)
            0.0f,                      // 高さ（水平）
            Mathf.Sin(angle) * radius  // Z座標 (円のZ方向)
        );

        // 円運動を計算する力
        Vector3 force = speed * circularDirection.normalized;
        rbEgg.AddForce(force, ForceMode.Impulse);

        // 衝突時に削除するコンポーネントを追加
        thrownCutter.AddComponent<DestroyOnCollision>();

        // 指定時間後に削除
        Destroy(thrownCutter, 2.0f);

        // 円運動の開始
        StartCoroutine(CircularMotion(thrownCutter, rbEgg, 12.0f, 1.0f));
        isThrowing = false;
    }

    // 円運動を作るコルーチン
    private IEnumerator CircularMotion(GameObject cutter, Rigidbody rb, float speed, float moveTime)
    {
        float elapsedTime = 0f; // 経過時間を計測
        Vector3 direction = cutter.transform.forward; // 初期は前進

        while (true)
        {
            // Y軸を中心に高速回転
            cutter.transform.Rotate(0, 1200.0f * Time.deltaTime, 0, Space.Self);

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

    #region オノマトペ情報
    private void ChickenWalkData()
    {
        GenerateWalkOnomatopoeia();
    }

    private void ChickenAttackData()
    {
        GenerateAttackOnomatopoeia();
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(spawnPoint.position + new Vector3(0f, 1f, 0f), spawnPoint.position + spawnPoint.forward * enemy.Status.StatusData.attackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(enemy.transform.position, minDistance);
    }
    #endregion
}
