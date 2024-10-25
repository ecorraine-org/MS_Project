using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerState
{
    private bool isHammerSkillExecuting = false; 
    //攻撃test
    public UnityEngine.Vector3 attackSize = new UnityEngine.Vector3(1f, 1f, 1f);
    UnityEngine.Vector3 defaultAttackSize;
    UnityEngine.Vector3 attackAreaPos;
    public UnityEngine.Vector3 offsetPos;
    public float attackDamage;
    public LayerMask enemyLayer;

    enum SkillState
    {
        Charge,
        Execute,
        Finish,

    }
    SkillState skillState = SkillState.Execute;

    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(true);

        defaultAttackSize = attackSize;

        base.Init(_playerController);
        Debug.Log("スキルステート");

        //スキルを発動する
        playerController.SkillManager.UseSkill((PlayerSkill)playerModeManager.Mode);

        if (playerSkillManager.SkillData.dicSkill[(PlayerSkill)playerModeManager.Mode].canCharge)
        {
            skillState= SkillState.Charge;
        }

        //switch (playerModeManager.Mode)
        //{
        //    case PlayerMode.None:
        //        playerController.SkillManager.UseSkill(PlayerSkill.Sword);
        //        break;
        //    case PlayerMode.Sword:
        //        playerController.SkillManager.UseSkill(PlayerSkill.Sword);
        //        break;
        //    case PlayerMode.Hammer:
        //        playerController.SkillManager.UseSkill(PlayerSkill.Hammer);
        //        break;
        //    case PlayerMode.Spear:
        //        playerController.SkillManager.UseSkill(PlayerSkill.Spear);
        //        break;
        //    case PlayerMode.Gauntlet:
        //        playerController.SkillManager.UseSkill(PlayerSkill.Gauntlet);
        //        break;
        //    default:
        //        break;
        //}
    }

    public override void Tick()
    {
        bool canCharge = playerSkillManager.SkillData.dicSkill[(PlayerSkill)playerModeManager.Mode].canCharge;
        bool isCharging = playerSkillManager.DicIsCharge[(PlayerSkill)playerModeManager.Mode];

       // switch()

       


        //条件
        //ボタンを長押し
        //該当のモードチャージ可能

        if (inputManager.GetSkillPressed()
           &&canCharge)
        {
            playerSkillManager.ExecuteSkillCharge((PlayerSkill)playerModeManager.Mode);

            //今後秒ごとに
            statusManager.TakeDamage(0.05f);
            attackSize *= 1.01f;

            //移動速度取得//test
            float moveSpeed = statusManager.StatusData.velocity/2;

            //方向取得
            Vector2 inputDirec = inputManager.GetMoveDirec();

            rb.velocity = new UnityEngine.Vector3(inputDirec.x * moveSpeed, rb.velocity.y, inputDirec.y * moveSpeed);

            return;
        }

        //移動可能
        //方向変えないようにする

        if (inputManager.GetSkillReleased() && isCharging)
        {
            playerSkillManager.ExecuteSkillChargeFinished((PlayerSkill)playerModeManager.Mode);

            Attack();

          //  attackSize = defaultAttackSize;

            return;
        }


        Attack();

        //アニメーション終了、アイドルへ遷移
        AnimatorStateInfo stateInfo = spriteAnim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 1f
            &&(!canCharge||( canCharge&&!isCharging))
            )
        {
            playerController.StateManager.TransitionState(StateType.Idle);
        }
    }

 

    public override void FixedTick()
    {

    }

    public override void Exit()
    {
        playerController.SpriteRenderer.color = Color.white;

        attackSize= defaultAttackSize;
    }

    public void Attack()
    {

        attackAreaPos = transform.position;

        //左右反転か
        offsetPos.x = spriteRenderer.flipX ? Mathf.Abs(offsetPos.x) : -Mathf.Abs(offsetPos.x);

        attackAreaPos += offsetPos;

        //コライダーの検出
        playerController.AttackCollider.DetectColliders(attackAreaPos, attackSize, attackDamage, enemyLayer,false);

    }

    /// <summary>
    /// 描画test
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackAreaPos, attackSize);
    }

    //private void AttractEnemiesToPlayer()
    //{

    //    float attractRadius = 200f;
    //    float distanceBetweenEnemies = 1f; 

       
    //    Collider[] enemies = Physics.OverlapSphere(transform.position, attractRadius, LayerMask.GetMask("Enemy"));
    //    List<Transform> enemyTransforms = new List<Transform>();

    //    foreach (Collider enemy in enemies)
    //    {
    //        enemyTransforms.Add(enemy.transform);
    //    }


    //    Vector3 targetCenter = transform.position + transform.forward * 2f;
    //    float angleStep = 360f / enemyTransforms.Count; 

    //    for (int i = 0; i < enemyTransforms.Count; i++)
    //    {
      
    //        float angle = i * angleStep;
    //        Vector3 targetPosition = targetCenter + Quaternion.Euler(0, angle, 0) * Vector3.forward * distanceBetweenEnemies;

    //        StartCoroutine(MoveEnemyToTarget(enemyTransforms[i], targetPosition));
    //    }
    //}

    //private void AttractEnemiesToPlayer()
    //{
    //    float attractRadius = 20f; 


    //    Collider[] enemies = Physics.OverlapSphere(transform.position, attractRadius, LayerMask.GetMask("Enemy"));

    //    foreach (Collider enemy in enemies)
    //    {

    //        Vector3 enemyPosition = enemy.transform.position;

   
    //        Vector3 targetPosition = transform.position + playerController.CurDirecVector * 2f; 

        
    //        StartCoroutine(MoveEnemyToTarget(enemy.transform, targetPosition));
    //    }
    //}

    //private IEnumerator MoveEnemyToTarget(Transform enemy, Vector3 targetPosition)
    //{
    //    float duration = 2.0f; 
    //    float elapsed = 0f;

    //    Vector3 startingPosition = enemy.position;

    //    while (elapsed < duration)
    //    {
    //        elapsed += Time.deltaTime;
    //        float t = elapsed / duration;

    //        enemy.position = Vector3.Lerp(startingPosition, targetPosition, t);
    //        yield return null;
    //    }

    //    enemy.position = targetPosition;
    //}
}
