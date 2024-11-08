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
    protected ObjectStatusHandler enemyStatus;

    protected Transform player;
    protected float distanceToPlayer;

    protected GameObject collector;

    public virtual void Move()
    {
        if (enemy != null && enemy.MovementInput.magnitude > 0.1f && enemy.Status.MoveSpeed > 0)
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
        if (distanceToPlayer < enemy.Status.StatusData.chaseDistance)
        {
            Vector3 direction = player.position - enemy.transform.position;
            // 進む方向に向く
            Quaternion newRotation = Quaternion.LookRotation(direction.normalized);
            newRotation.x = 0;
            enemy.transform.rotation = newRotation;

            if (distanceToPlayer <= enemy.Status.StatusData.attackDistance)
            {
                enemy.OnMovementInput?.Invoke(Vector3.zero);

                // 攻撃
                enemy.State.TransitionState(ObjectStateType.Attack);
            }
            else
            {
                // 追跡
                enemy.OnMovementInput?.Invoke(direction.normalized);
            }
        }
        else
        {
            // 停止
            enemy.OnMovementInput?.Invoke(Vector3.zero);
        }

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
