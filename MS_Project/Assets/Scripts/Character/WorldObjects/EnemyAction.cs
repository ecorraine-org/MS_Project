using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーアクション基底クラス
/// </summary>
public abstract class EnemyAction : MonoBehaviour
{
    protected EnemyController enemy;
    protected ObjectStatusHandler enemyStatus;

    protected Transform player;
    protected float distanceToPlayer;

    protected GameObject collector;

    public virtual void Move()
    {
        if (enemy != null)
            enemy.RigidBody.velocity = enemy.MovementInput * enemy.Status.MoveSpeed;
    }

    public abstract void Attack();

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyController>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject;
    }

    protected void Start()
    {
        distanceToPlayer = Vector3.Distance(player.position, transform.position);
    }

    protected void Update()
    {
        distanceToPlayer = Vector3.Distance(player.position, transform.position);
    }

    public EnemyController Enemy
    {
        get { return enemy; }
        set { enemy = value; }
    }

    public ObjectStatusHandler EnemyStatus
    {
        get { return enemyStatus; }
        set { enemyStatus = value; }
    }
}
