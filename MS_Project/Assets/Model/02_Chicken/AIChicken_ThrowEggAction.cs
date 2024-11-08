using UnityEngine;

public class AIChicken_ThrowEggAction : EnemyAction
{
    public GameObject eggPrefab;         // 投げるプレハブ
    public Transform spawnPoint;         // プレハブの生成位置
    public float throwForce = 10f;       // 投げる力
    public int throwCount = 3;           // 連続して投げる回数（publicで調整可能）
    public float throwInterval = 0.5f;   // 投擲の間隔（秒）

    private int currentThrow = 0;        // 現在の投擲回数
    private bool isThrowing = false;     // 投擲中フラグ

    public override void Attack()
    {
        // 逃げる
        if (distanceToPlayer < enemy.Status.StatusData.attackDistance)
        {
            enemy.CanAttack = false;
            enemy.Anim.Play("Walk");
            Vector3 direction = player.position - enemy.transform.position;
            // 進む方向に向く
            Quaternion newRotation = Quaternion.LookRotation(-direction.normalized);
            newRotation.x = 0;
            enemy.transform.rotation = newRotation;

            Vector3 newMovement = enemy.transform.forward * 0.88f * Time.deltaTime;
            enemy.RigidBody.MovePosition(enemy.RigidBody.position + newMovement);
        }
        else
            ThrowEgg();
    }

    // アニメーションイベントから呼び出すメソッド
    public void StartThrowSequence()
    {
        if (!isThrowing)
        {
            currentThrow = 0;
            isThrowing = true;
            InvokeRepeating(nameof(ThrowEgg), 0f, throwInterval);
        }
    }

    //--------------------------------------------------------------
    // 斜方投射
    private void ThrowEgg()
    {
        // プレハブのインスタンスを生成
        GameObject thrownEgg = Instantiate(eggPrefab, spawnPoint.position, spawnPoint.rotation, collector.transform);
        Rigidbody rbEgg = thrownEgg.GetComponent<Rigidbody>();
        rbEgg.useGravity = true;

        collector.GetComponent<Collector>().otherObjectPool.Add(thrownEgg);

        // 投げる力の大きさ
        float forceMagnitude = 20.0f;
        float gravityScale = 50.0f; // 重力を通常より強くする

        // 指定した角度で前方に飛ばす
        float angle = 85.0f; // 前方に対して30度上向きに飛ばす
        float radianAngle = angle * Mathf.Deg2Rad;

        // 指定角度に応じた方向を計算
        Vector3 forceDirection = (spawnPoint.forward * Mathf.Cos(radianAngle) + spawnPoint.up * Mathf.Sin(radianAngle)).normalized;

        // 力を計算して加える
        Vector3 force = forceMagnitude * forceDirection;
        rbEgg.AddForce(force, ForceMode.Impulse);

        // 重力の強化（重力を上乗せするために下方向の追加力を適用）
        Vector3 additionalGravity = Physics.gravity * (gravityScale - 1.0f); // gravityScaleが1なら通常重力、2なら2倍
        rbEgg.AddForce(additionalGravity, ForceMode.Acceleration);

        // 回転を加える
        Vector3 torque = new Vector3(400.0f, spawnPoint.forward.y, 0.0f); // Z軸を中心に回転するトルク
        rbEgg.AddTorque(torque, ForceMode.Impulse);
    }
}
