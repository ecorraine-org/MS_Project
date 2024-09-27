using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoEnemyController : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�^�X")]
    float currentSpeed = 0;
    [SerializeField, Header("�U��")]
    bool isAttack = true;
    //�N�[���^�C��
    float attackCoolDuration = 1;

    public Vector3 MovementInput { get; set; }

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (MovementInput.magnitude > 0.1f && currentSpeed >= 0)
        {
            rb.velocity = MovementInput * currentSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void Attack()
    {
        if (isAttack)
        {
            Debug.Log("�G�U��");
            isAttack = false;
            StartCoroutine(nameof(AttackCoroutine));
        }
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackCoolDuration);
        isAttack = true;
    }
}
