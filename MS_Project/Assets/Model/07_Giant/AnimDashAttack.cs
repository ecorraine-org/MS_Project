using UnityEngine;

public class AnimDashAttack : MonoBehaviour
{
    public float dashSpeed = 10f;       // �_�b�V���̃X�s�[�h
    public float dashDuration = 0.5f;   // �_�b�V���̎�������
    private bool isDashing = false;
    private float dashTime;
    private Vector3 dashDirection;

    void Update()
    {
        if (isDashing)
        {
            // �_�b�V�����͑��x�Ɋ�Â��đO���Ɉړ�
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);

            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;  // �_�b�V�����I��
            }
        }
    }

    // �A�j���[�V�����C�x���g�ŌĂяo���_�b�V���J�n�֐�
    public void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        dashDirection = transform.forward;  // �O���Ɍ������Ĉړ�
    }
}
