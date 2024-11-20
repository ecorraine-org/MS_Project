using UnityEngine;

public class AIChicken_ThrowEgg : EnemyAction
{
    [SerializeField, Header("投げるプレハブ")]
    public GameObject projectilePrefab;
    [SerializeField, Header("プレハブの生成位置")]
    public Transform spawnPoint;
    [SerializeField, Header("投擲角度（上向き）")]
    float upAngle = 30f;
    [SerializeField, Header("投擲角度（プレイヤー向き）")]
    float playerAngle = 30f;

    [SerializeField, Header("投げる力")]
    float throwForce = 10f;

    [SerializeField] GameObject ExplosionPrefab;

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


    protected override void Start()
    {
        base.Start();

        maxDistance = enemy.EnemyStatus.StatusData.attackDistance;
        minDistance = enemy.EnemyStatus.StatusData.attackDistance / 1.3f;
    }

    public override void Chase()
    {
        if (distanceToPlayer <= EnemyStatus.StatusData.chaseDistance)
        {
            Vector3 direction = player.position - transform.position;
            // 進む方向に向く
            Quaternion forwardRotation = Quaternion.LookRotation(direction.normalized);
            forwardRotation.x = 0f;
            transform.rotation = forwardRotation;

            if (distanceToPlayer < minDistance)
            {
                Quaternion backwardRotation = Quaternion.LookRotation(-direction.normalized);
                backwardRotation.x = 0f;
                transform.rotation = backwardRotation;

                if (enemy.AllowAttack || enemy.IsAttacking)
                {
                    enemy.AllowAttack = false;
                    enemy.IsAttacking = false;
                }

                // 逃げる
                enemy.OnMovementInput?.Invoke(-direction.normalized / 2);
            }
            else if (distanceToPlayer <= maxDistance && distanceToPlayer > minDistance)
            {
                enemy.OnMovementInput?.Invoke(Vector3.zero);

                // 攻撃
                enemy.OnAttack?.Invoke();
            }
            else if (distanceToPlayer > maxDistance)
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

    //--------------------------------------------------------------
    // 斜方投射
    private void ThrowEgg()
    {
        // プレハブのインスタンスを生成
        GameObject thrownEgg = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation, collector.transform);
        Rigidbody rbEgg = thrownEgg.GetComponent<Rigidbody>();
        rbEgg.useGravity = true;

        collector.GetComponent<Collector>().otherObjectPool.Add(thrownEgg);

        Quaternion rotateToPlayer = Quaternion.LookRotation((player.position - spawnPoint.position).normalized);
        spawnPoint.rotation = rotateToPlayer;

        float radianPlayerAngle = playerAngle * Mathf.Deg2Rad;
        float radianUpAngle = upAngle * Mathf.Deg2Rad;
        // 指定した角度に飛ばす
        Vector3 playerDirection = spawnPoint.forward /*+ (Vector3.forward * Mathf.Cos(radianPlayerAngle))*/;
        // 前方に対してn度上向きに飛ばす
        Vector3 upDirection = spawnPoint.up * Mathf.Sin(radianUpAngle);
        // 指定角度に応じた方向を計算
        Vector3 forceDirection = (playerDirection + upDirection).normalized;

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
            aiChickenThrowEgg = FindObjectOfType<AIChicken_ThrowEgg>(); // AIChicken_ThrowEggのインスタンスを取得
        }

        private void OnCollisionEnter(Collision collision)
        {
            // プレイヤーと衝突した場合に卵を削除
            if (collision.gameObject.CompareTag("Player"))
            {
                if (aiChickenThrowEgg != null)
                {
                    Instantiate(aiChickenThrowEgg.ExplosionPrefab, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
            // プレイヤー以外と衝突した場合にも卵を削除
            else if (!collision.gameObject.CompareTag("Player"))
            {
                if (aiChickenThrowEgg != null)
                {
                    Instantiate(aiChickenThrowEgg.ExplosionPrefab, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
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
        Gizmos.DrawLine(spawnPoint.position + new Vector3(0f, 1f, 0f), spawnPoint.position + spawnPoint.forward * enemy.EnemyStatus.StatusData.attackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(enemy.transform.position, minDistance);
    }
    #endregion
}
