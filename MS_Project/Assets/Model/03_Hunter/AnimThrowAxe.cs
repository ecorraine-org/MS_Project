using UnityEngine;

public class AnimThrowAxe : EnemyAction
{
    public GameObject axePrefab; // 投げるプレハブ
    public Transform spawnPoint; // プレハブの生成位置
    public float throwForce = 10f; // 投げる力

    public override void Move()
    {
    }

    public override void SkillAttack()
    {
    }

    // アニメーションイベントから呼び出すメソッド
    public void ThrowAxe()
    {
        if (axePrefab != null && spawnPoint != null)
        {
            // プレハブのインスタンスを生成
            GameObject thrownAxe = Instantiate(axePrefab, spawnPoint.position, spawnPoint.rotation);
            // 投げる方向に力を加える
            Rigidbody rbAxe = thrownAxe.GetComponent<Rigidbody>();
            if (rbAxe != null)
            {
                rbAxe.AddForce(spawnPoint.forward * throwForce, ForceMode.Impulse);
            }
        }
    }
}
