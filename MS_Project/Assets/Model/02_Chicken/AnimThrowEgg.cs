using UnityEngine;

public class AnimThrowEgg : MonoBehaviour
{
    public GameObject axePrefab;         // 投げるプレハブ
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
            InvokeRepeating(nameof(ThrowAxe), 0f, throwInterval);
        }
    }

    // 投擲処理
    private void ThrowAxe()
    {
        if (currentThrow < throwCount)
        {
            if (axePrefab != null && spawnPoint != null)
            {
                // プレハブのインスタンスを生成
                GameObject thrownAxe = Instantiate(axePrefab, spawnPoint.position, spawnPoint.rotation);
                // 投げる方向に力を加える
                Rigidbody rb = thrownAxe.GetComponent<Rigidbody>();
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
            CancelInvoke(nameof(ThrowAxe));
            isThrowing = false;
        }
    }
}
