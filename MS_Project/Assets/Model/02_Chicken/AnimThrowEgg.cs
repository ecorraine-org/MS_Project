using UnityEngine;

public class AnimThrowEgg : EnemyAction
{
    public GameObject axePrefab;         // 投げるプレハブ
    public Transform spawnPoint;         // プレハブの生成位置
    public float throwForce = 10f;       // 投げる力
    public int throwCount = 3;           // 連続して投げる回数（publicで調整可能）
    public float throwInterval = 0.5f;   // 投擲の間隔（秒）

    private int currentThrow = 0;        // 現在の投擲回数
    private bool isThrowing = false;     // 投擲中フラグ

    //--------------------------------------------------------------
    //斜方投射

    public override void Move()
    {
        if (distanceToPlayer < EnemyStatus.StatusData.attackDistance)
        {
            // 逃げる
            Vector3 movement = -transform.forward * 0.88f * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }
    public override void SkillAttack()
    {
    }

    // アニメーションイベントから呼び出すメソッド
    public void StartThrowSequence()
    {
        if (!isThrowing)
        {
            currentThrow = 0;
            isThrowing = true;
            InvokeRepeating(nameof(ThrowEggAI), 0f, throwInterval);
        }
    }

    private void ThrowEggAI()
    {
        // プレハブのインスタンスを生成
        GameObject thrownAxe = Instantiate(axePrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rbEgg = thrownAxe.GetComponent<Rigidbody>();
        rbEgg.useGravity = true;

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
        Vector3 torque = new Vector3(0, 0, 40.0f); // Z軸を中心に回転するトルク
        rbEgg.AddTorque(torque, ForceMode.Impulse);
    }
}
