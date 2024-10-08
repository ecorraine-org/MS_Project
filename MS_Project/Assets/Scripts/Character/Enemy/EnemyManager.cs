using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    [SerializeField, Header("プレイヤー")]
    Transform player;

    [SerializeField, Header("ステータスマネージャー")]
    EnemyStatusManager statusManager;

    public UnityEvent<Vector3> OnMovementInput;

    public UnityEvent OnAttack;

    public UnityEvent OnDamaged;

    BoxCollider boxCollider;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        statusManager = gameObject.transform.GetChild(0).gameObject.GetComponent<EnemyStatusManager>();
    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        BoxCollider collider = gameObject.transform.GetChild(1).gameObject.GetComponent<BoxCollider>();
        boxCollider.center = collider.center / 3;
        boxCollider.size = collider.size / 3;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < statusManager.StatusData.fChaseDistance)
        {

            if (distance <= statusManager.StatusData.fAttackDistance)
            {
                OnMovementInput?.Invoke(Vector3.zero);
                // 攻撃
                OnAttack?.Invoke();
            }
            else
            {
                // 追跡
                Vector3 direction = player.position - transform.position;
                // 進む方向に向く
                Quaternion newRotation = Quaternion.LookRotation(direction.normalized);
                newRotation.x = 0;
                transform.rotation = newRotation;

                OnMovementInput?.Invoke(direction.normalized);
            }
        }
        else
        {
            // 停止
            OnMovementInput?.Invoke(Vector3.zero);
        }

        if (Debug.isDebugBuild)
        {
            Debug.DrawRay(transform.position, (player.position - transform.position) * 2, Color.blue);
            Debug.DrawRay(transform.position, transform.forward * 3, Color.red);
        }

    }
}
