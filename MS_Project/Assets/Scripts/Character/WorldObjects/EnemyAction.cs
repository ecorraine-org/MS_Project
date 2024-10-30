using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAction : MonoBehaviour
{
    protected ObjectStatusHandler enemyStatus;
    protected Animator animator;
    protected Transform player;
    protected Rigidbody rb;
    protected float distanceToPlayer;

    public abstract void SkillAttack();

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

    public void SetPlayer(Transform _player)
    {
        this.player = _player;
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
