using UnityEngine;

public class AnimThrowEgg : EnemyAction
{
    public GameObject eggPrefab;         // 投げるプレハブ
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
            InvokeRepeating(nameof(ThrowEgg), 0f, throwInterval);
        }
    }

    private void ThrowEgg()
    {
        //if (currentThrow < throwCount)
        {
            // プレハブのインスタンスを生成
            GameObject thrownAxe = Instantiate(axePrefab, spawnPoint.position, spawnPoint.rotation);
            // 投げる方向に力を加える
            Rigidbody rbEgg = thrownAxe.GetComponent<Rigidbody>();
            rbEgg.useGravity = true;

            Vector3 forceDirection = new Vector3(1.0f, 1.0f, 0f);

            // 上の向きに加わる力の大きさを定義
            float forceMagnitude = 10.0f;

            // 向きと大きさからSphereに加わる力を計算する
            Vector3 force = forceMagnitude * forceDirection;

            // 力を加えるメソッド
            // ForceMode.Impulseは撃力
            rbEgg.AddForce(force, ForceMode.Impulse);

        }
        currentThrow++;

    }

    // 投擲処理
    private void ThrowEgg()
    {
        if (currentThrow < throwCount)
        {
            if (eggPrefab != null && spawnPoint != null)
            {
                // プレハブのインスタンスを生成
                GameObject thrownEgg = Instantiate(eggPrefab, spawnPoint.position, spawnPoint.rotation, collector.transform);
                Rigidbody rbEgg = thrownEgg.GetComponent<Rigidbody>();
                if (rbEgg != null)
                {
                    // 投げる方向に力を加える
                    rbEgg.AddForce(spawnPoint.forward * throwForce, ForceMode.Impulse);
                }
                collector.GetComponent<Collector>().otherObjectPool.Add(thrownEgg);
            }
            currentThrow++;
        }
        else
        {
            // 投擲が指定回数に達したら終了
            CancelInvoke(nameof(ThrowEgg));
            isThrowing = false;
        }
    }
}
