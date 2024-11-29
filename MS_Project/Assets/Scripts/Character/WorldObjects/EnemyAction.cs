using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

/// <summary>
/// エネミーアクション基底クラス
/// </summary>
public abstract class EnemyAction : MonoBehaviour
{
    protected EnemyController enemy;
    protected EnemyStatusHandler enemyStatus;
    protected ObjectStateHandler stateHandler;
    protected Animator animator;

    protected Transform player;
    protected float distanceToPlayer;

    protected GameObject collector;

    [SerializeField, Header("行動パターンビヘイビア")]
    protected ActionPatternList[] actionPatternList;

    //*******************
    //List処理

    //リスト用タイマー
    protected float listTimer = 0.0f;

    //何番目のリストか
    //0:形態1 1:形態2
    protected int listIndex = 0;

    //アクションリストのインデックス
    //初回-1 0攻撃1　1攻撃2
    protected int actionStage = -1;

    //実行しようとする行動処理
    protected Action currentUpdateAction;
    //*******************

    public virtual void Init(EnemyController _enemy)
    {
        enemy = GetComponentInParent<EnemyController>();
        enemyStatus = enemy.EnemyStatus;
        stateHandler = enemy.State;
        animator = enemy.Anim;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject;
    }

    protected virtual void Start()
    {
        distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);
    }

    public virtual void Chase()
    {
        if (distanceToPlayer <= enemyStatus.StatusData.chaseDistance)
        {
            Vector3 direction = player.position - enemy.transform.position;
            // 進む方向に向く
            Quaternion forwardRotation = Quaternion.LookRotation(direction.normalized);
            forwardRotation.x = 0f;
            enemy.transform.rotation = forwardRotation;

            if (distanceToPlayer <= enemyStatus.StatusData.attackDistance)
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

        if (actionPatternList != null) listTimer += Time.deltaTime;
    }

    protected void GenerateAttackOnomatopoeia()
    {
        enemy.GenerateOnomatopoeia(enemy.gameObject, enemy.EnemyStatus.StatusData.onomatoAttack);
    }

    protected void GenerateWalkOnomatopoeia()
    {
        enemy.GenerateOnomatopoeia(enemy.gameObject, enemy.EnemyStatus.StatusData.onomatoWalk);
    }

    /// <summary>
    /// 次の行動に設定する
    /// </summary>
    protected void AddActionStage()
    {
        actionStage++;

        //超えたら戻る
        if (actionStage >= actionPatternList[listIndex].GetActionPattern().Length)
            actionStage = 0;
    }

    protected bool CheckListTimer()
    {
        //actionStage=-1の場合、初めてループに入って、実行する
        //actionStage>-1の場合、時間達したら実行する
        if ((actionStage == -1)
            || (listTimer >= actionPatternList[listIndex].GetActionPattern()[actionStage].recoveryTime))
        {
            listTimer = 0;
            //次の行動
            AddActionStage();

            return true;
        }


        return false;

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
