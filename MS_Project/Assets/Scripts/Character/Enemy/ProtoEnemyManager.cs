using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProtoEnemyManager : MonoBehaviour
{
    public UnityEvent<Vector3> OnMovementInput;

    public UnityEvent OnAttack;

    [SerializeField, Header("�v���C���[")]
    Transform player;

    [SerializeField, Header("�ǐՋ���")]
    float chaseDistance = 3f;

    [SerializeField, Header("�U������")]
    float attackDistance = 0.8f;

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < chaseDistance)
        {

            if (distance <= attackDistance)
            {
                OnMovementInput?.Invoke(Vector3.zero);
                //�U��
                OnAttack?.Invoke();
            }
            else
            {
                //�ǐ�
                Vector3 direction = player.position - transform.position;
                OnMovementInput?.Invoke(direction.normalized);
            }
        }
        else
        {
            //��~
            OnMovementInput?.Invoke(Vector3.zero);
        }
    }
}
