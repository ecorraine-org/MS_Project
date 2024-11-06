using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAction : MonoBehaviour
{
    protected EnemyController enemy;
    protected ObjectStatusHandler enemyStatus;

    protected Animator animator;
    protected Transform player;
    protected Rigidbody rb;
    protected float distanceToPlayer;

    protected GameObject collector;

    public abstract void Move();
    public abstract void SkillAttack();

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyController>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject;
    }

    private void Start()
    {
        distanceToPlayer = Vector3.Distance(player.position, transform.position);
    }

    public ObjectStatusHandler EnemyStatus
    {
        get { return enemyStatus; }
        set { enemyStatus = value; }
    }

    public void SetAnimator(Animator _animator)
    {
        this.animator = _animator;
    }

    public void SetRigidbody(Rigidbody _rb)
    {
        this.rb = _rb;
    }

    public void SetDistanceToPlayer(float _distanceToPlayer)
    {
        this.distanceToPlayer = _distanceToPlayer;
    }
}
