using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// エネミーアクション基底クラス
/// </summary>
public abstract class EnemyAction : MonoBehaviour
{
    protected EnemyController enemy;
    protected EnemyStatusHandler enemyStatus;

    protected Transform player;
    protected float distanceToPlayer;

    protected GameObject collector;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyController>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject;
    }

    protected virtual void Start()
    {
        distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);
    }

    public virtual void Chase()
    {
        if (distanceToPlayer <= EnemyStatus.StatusData.chaseDistance)
        {
            Vector3 direction = player.position - enemy.transform.position;
            // 進む方向に向く
            Quaternion forwardRotation = Quaternion.LookRotation(direction.normalized);
            forwardRotation.x = 0f;
            enemy.transform.rotation = forwardRotation;

            if (distanceToPlayer <= EnemyStatus.StatusData.attackDistance)
            {
                enemy.AllowAttack = true;
                enemy.OnMovementInput?.Invoke(Vector3.zero);

                // 攻撃
                enemy.OnAttack?.Invoke();
            }
            else
            {
                if (enemy.AllowAttack || enemy.IsAttacking)
                {
                    enemy.AllowAttack = false;
                    enemy.IsAttacking = false;
                }

                // 追跡
                enemy.OnMovementInput?.Invoke(direction.normalized);
            }
        }
        else
        {
            if (enemy.AllowAttack || enemy.IsAttacking)
            {
                enemy.AllowAttack = false;
                enemy.IsAttacking = false;
            }

            // 停止
            enemy.OnMovementInput?.Invoke(Vector3.zero);
        }
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);
    }

    public EnemyController Enemy
    {
        get { return enemy; }
        set { enemy = value; }
    }

    public EnemyStatusHandler EnemyStatus
    {
        get { return enemyStatus; }
        set { enemyStatus = value; }
    }
}
