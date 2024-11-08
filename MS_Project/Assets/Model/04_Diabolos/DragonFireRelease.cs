using UnityEngine;

public class DragonFireRelease : MonoBehaviour
{
    public GameObject firePrefab;        // 投げるプレハブ
    public Transform spawnPoint;         // プレハブの生成位置
    public float throwForce = 10f;       // 投げる力
    public int throwCount = 3;           // 連続して投げる回数（publicで調整可能）
    public float throwInterval = 0.5f;   // 投擲の間隔（秒）

    private int currentThrow = 0;        // 現在の投擲回数
    private bool isThrowing = false;     // 投擲中フラグ

    // アニメーションイベントから呼び出すメソッド
    public void StartThrowSequence()
    {
        if (!isThrowing)
        {
            currentThrow = 0;
            isThrowing = true;
            InvokeRepeating(nameof(ThrowFire), 0f, throwInterval);
        }
    }

    // 投擲処理
    // 連続火遁
    private void ThrowFire()
    {
        if (currentThrow < throwCount)
        {
            if (firePrefab != null && spawnPoint != null)
            {
                // プレハブのインスタンスを生成
                GameObject thrownFire = Instantiate(firePrefab, spawnPoint.position, spawnPoint.rotation);
                // 投げる方向に力を加える
                Rigidbody rb = thrownFire.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(spawnPoint.forward * throwForce, ForceMode.Impulse);
                }
            }
            currentThrow++;
        }
        else
        {
            // 投擲が指定回数に達したら終了
            CancelInvoke(nameof(ThrowFire));
            isThrowing = false;
        }
    }

    // シングル火遁
    public void SingleFire()
    {
        if (firePrefab != null && spawnPoint != null)
        {
            // プレハブのインスタンスを生成
            GameObject thrownFire = Instantiate(firePrefab, spawnPoint.position, spawnPoint.rotation);
            // 投げる方向に力を加える
            Rigidbody rbFire = thrownFire.GetComponent<Rigidbody>();
            if (rbFire != null)
            {
                rbFire.AddForce(spawnPoint.forward * throwForce, ForceMode.Impulse);
            }
        }
    }
}