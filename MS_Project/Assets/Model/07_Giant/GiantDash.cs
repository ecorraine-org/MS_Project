using UnityEngine;

public class GiantDash : MonoBehaviour
{
    public float dashSpeed = 10f;     // ダッシュのスピード
    public float dashDuration = 0.5f; // ダッシュの持続時間
    private bool isDashing = false;   // ダッシュ中かどうかのフラグ
    private float dashTimer = 0f;     // ダッシュの残り時間

    private void Update()
    {
        if (isDashing)
        {
            // ダッシュ中の処理
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);

            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                EndDash();
            }
        }
    }

    // アニメーションイベントで呼び出されるダッシュ開始メソッド
    public void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
    }

    // ダッシュ終了処理
    private void EndDash()
    {
        isDashing = false;
    }
}
