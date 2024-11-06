using UnityEngine;

public class AnimDashAttack : MonoBehaviour
{
    public float dashSpeed = 10f;       // ダッシュのスピード
    public float dashDuration = 0.5f;   // ダッシュの持続時間
    private bool isDashing = false;
    private float dashTime;
    private Vector3 dashDirection;

    void Update()
    {
        if (isDashing)
        {
            // ダッシュ中は速度に基づいて前方に移動
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);

            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;  // ダッシュを終了
            }
        }
    }

    // アニメーションイベントで呼び出すダッシュ開始関数
    public void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        dashDirection = transform.forward;  // 前方に向かって移動
    }
}
