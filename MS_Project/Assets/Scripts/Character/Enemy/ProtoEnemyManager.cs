using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProtoEnemyManager : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�^�X�}�l�[�W���[")]
    ProtoEnemyStatusManager statusManager;

    public UnityEvent<Vector3> OnMovementInput;

    public UnityEvent OnAttack;

    [SerializeField, Header("�v���C���[")]
    Transform player;

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < statusManager.StatusData.chaseDistance)
        {

            if (distance <= statusManager.StatusData.attackDistance)
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
