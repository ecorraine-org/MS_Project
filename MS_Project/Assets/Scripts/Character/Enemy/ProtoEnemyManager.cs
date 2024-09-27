using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProtoEnemyManager : MonoBehaviour
{
    public UnityEvent<Vector3> OnMovementInput;

    public UnityEvent OnAttack;

    [SerializeField, Header("ƒvƒŒƒCƒ„[")]
    Transform player;

    [SerializeField, Header("’ÇÕ‹——£")]
    float chaseDistance = 3f;

    [SerializeField, Header("UŒ‚‹——£")]
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
                //UŒ‚
                OnAttack?.Invoke();
            }
            else
            {
                //’ÇÕ
                Vector3 direction = player.position - transform.position;
                OnMovementInput?.Invoke(direction.normalized);
            }
        }
        else
        {
            //’â~
            OnMovementInput?.Invoke(Vector3.zero);
        }
    }
}
