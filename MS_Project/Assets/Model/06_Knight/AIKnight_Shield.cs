using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKnight_Shield : EnemyAction
{
    [SerializeField, Header("防御率"), Range(0f, 100f)]
    int guardChance = 50;

    private Vector3 direction;

    public void Update()
    {
        distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Skill"))
        {
            enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage/2, false);
        }

    }


    // 確率で防御
    public void Guard()
    {
        int random = Random.Range(1, 10);
        if (random >= guardChance / 10)
        {
            enemy.Anim.Play("Guard", 0, 0f);
        }
    }

    public void WalkInit()
    {
        enemy.Anim.Play("Walk");
    }

    public void WalkTick()
    {
        //ダメージチェック
        if (stateHandler.CheckHit()) return;

        enemy.Move();

        // enemyの現在の角度を基にした前方向
        Vector3 forwardDirection = enemy.transform.forward;
        forwardDirection.y = 0f;// 地面に沿った移動

        //適度に距離を置く
        if (distanceToPlayer >= enemy.Status.StatusData.attackDistance)
        {
            // 追跡
            enemy.OnMovementInput?.Invoke(forwardDirection.normalized);
        }
        else if (distanceToPlayer < enemy.Status.StatusData.attackDistance * 0.5f)
        {
            // 後ろ
            //enemy.OnMovementInput?.Invoke(-forwardDirection.normalized / 5.0f);
        }

        //ちょっとずつ見る
        direction = player.position - enemy.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        enemy.transform.rotation = Quaternion.Slerp(
        enemy.transform.rotation,
        targetRotation,
        0.1f // 補間率（1.0fで即時、0.0fで変化なし）
    );

        //攻撃へ遷移
        if (distanceToPlayer <= enemyStatus.StatusData.attackDistance && enemy.AllowAttack)
        {
            //クールダウン
            enemy.StartAttackCoroutine();

            stateHandler.TransitionState(ObjectStateType.Attack);
            return;
        }
    }

    public void AttackkInit()
    {
        enemy.Anim.Play("PostSkill");
        currentUpdateAction = AttackkTick;
    }

    public void AttackkTick()
    {
        enemy.AttackCollider.DetectColliders(enemy.Status.StatusData.damage, false);

        enemy.AttackCollider.CanHit = true;

        if (stateHandler.CheckDeath()) return;

        //ダメージチェック
        if (stateHandler.CheckHit()) return;

        if (enemy.AnimManager != null && enemy.AnimManager.IsAnimEnd)
        {
            enemy.State.TransitionState(ObjectStateType.Idle);
        }
    }

    #region オノマトペ情報
    private void KnightWalkData()
    {
        GenerateWalkOnomatopoeia();
    }

    private void KnightAttackData()
    {
        GenerateAttackOnomatopoeia();
    }
    #endregion
}
