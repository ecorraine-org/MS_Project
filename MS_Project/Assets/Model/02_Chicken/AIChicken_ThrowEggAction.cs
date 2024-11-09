using UnityEngine;

public class AIChicken_ThrowEggAction : EnemyAction
{
    [SerializeField, Header("投げるプレハブ")]
    public GameObject projectilePrefab;
    [SerializeField, Header("プレハブの生成位置")]
    public Transform spawnPoint;

    [SerializeField, Header("投げる力")]
    float throwForce = 10f;
    [SerializeField, Header("連続して投げる回数")]
    int throwCount = 3;
    [SerializeField, Header("投擲の間隔（秒）")]
    float throwInterval = 0.5f;

    [Tooltip("現在の投擲回数")]
    private int currentThrow = 0;
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

    protected override void Update()
    {
        base.Update();
    }

    public override void Move()
    {
        // 逃げる
        if (distanceToPlayer < minDistance)
        {
            enemy.CanAttack = false;

            Vector3 direction = player.position - enemy.transform.position;
            // 逃げる方向に向く
            Quaternion newRotation = Quaternion.LookRotation(-direction.normalized);
            newRotation.x = 0;
            enemy.transform.rotation = newRotation;

            enemy.OnMovementInput?.Invoke(-direction.normalized);
            //Vector3 newMovement = -enemy.transform.forward * 0.88f * Time.deltaTime;
            //enemy.RigidBody.MovePosition(enemy.RigidBody.position + newMovement);
        }

        base.Move();
    }

    public override void Attack()
    {
        // 逃げる
        if (distanceToPlayer > minDistance && distanceToPlayer <= maxDistance)
        {
            Vector3 direction = player.position - enemy.transform.position;
            // 攻撃方向に向く
            Quaternion newRotation = Quaternion.LookRotation(direction.normalized);
            newRotation.x = 0;
            enemy.transform.rotation = newRotation;

            StartThrowSequence();
        }
    }

    // アニメーションイベントから呼び出すメソッド
    public void StartThrowSequence()
    {
        if (!isThrowing)
        {
            currentThrow = 0;
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

        // 指定した角度で前方に飛ばす
        float angle = 30.0f; // 前方に対して30度上向きに飛ばす
        float radianAngle = angle * Mathf.Deg2Rad;

        // 指定角度に応じた方向を計算
        Vector3 forceDirection = (spawnPoint.forward * Mathf.Cos(radianAngle) + spawnPoint.up * Mathf.Sin(radianAngle)).normalized;

        // 力を計算して加える
        Vector3 force = throwForce * forceDirection;
        rbEgg.AddForce(force, ForceMode.Impulse);

        float gravityScale = 1.0f; // 重力を通常より強くする
        // 重力の強化（重力を上乗せするために下方向の追加力を適用）
        Vector3 additionalGravity = Physics.gravity * (gravityScale - 1.0f); // gravityScaleが1なら通常重力、2なら2倍
        rbEgg.AddForce(additionalGravity, ForceMode.Acceleration);

        // 回転を加える
        Vector3 torque = new Vector3(400.0f, spawnPoint.forward.y, 0.0f); // Z軸を中心に回転するトルク
        rbEgg.AddTorque(torque, ForceMode.Impulse);

        isThrowing = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(enemy.transform.position, minDistance);
    }

}
