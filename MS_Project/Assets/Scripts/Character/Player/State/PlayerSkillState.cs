using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerState
{
    [SerializeField, Header("コライダー")]
    HitCollider hitCollider;

    private bool isHammerSkillExecuting = false;
    [Tooltip("攻撃test")]
    public UnityEngine.Vector3 attackSize = new UnityEngine.Vector3(1f, 1f, 1f);
    UnityEngine.Vector3 defaultAttackSize;
    UnityEngine.Vector3 attackAreaPos;
    public UnityEngine.Vector3 offsetPos;
    public float attackDamage;
    public float FrenzyAttackDamage;
    public LayerMask enemyLayer;

    enum SkillState
    {
        [InspectorName("ノーマルスキル"), Tooltip("ノーマルスキル")]
        Normal,
        [InspectorName("長押しスキル"), Tooltip("長押しスキル")]
        Charge,
        [InspectorName("初期化"), Tooltip("初期化")]
        ExecuteInit,
        [InspectorName("実行"), Tooltip("実行")]
        Execute,
        [InspectorName("終了"), Tooltip("終了")]
        Finish,

    }
    [SerializeField, Header("スキル段階")]
    SkillState skillState = SkillState.Normal;

    public override void Init(PlayerController _playerController)
    {
        SetIsPerformDamage(true);

        defaultAttackSize = attackSize;
        defaultAttackSize = hitCollider.transform.localScale;

        base.Init(_playerController);

        playerController.AttackColliderV2.HitCollidersList = hitCollider;

        //スキルを発動する
        playerController.SkillManager.UseSkill((PlayerSkill)playerModeManager.Mode);

        //スキル段階初期化
        if (playerSkillManager.SkillData.dicSkill[(PlayerSkill)playerModeManager.Mode].canCharge)
        {
            skillState = SkillState.Charge;
        }
        else
        {
            skillState = SkillState.Normal;
        }
    }

    public override void Tick()
    {
        //捕食へ遷移
        if (playerStateManager.CheckEat()) return;

        //回避キャンセル
        //回避へ遷移
        if (playerSkillManager.SkillData.dicSkill[(PlayerSkill)playerModeManager.Mode].canCancel
            && playerStateManager.CheckDodge()) return;

        //  bool canCharge = playerSkillManager.SkillData.dicSkill[(PlayerSkill)playerModeManager.Mode].canCharge;
        // bool isCharging = playerSkillManager.DicIsCharge[(PlayerSkill)playerModeManager.Mode];

        AnimatorStateInfo stateInfo = spriteAnim.GetCurrentAnimatorStateInfo(0);

        switch (skillState)
        {
            case SkillState.Normal:

                Attack();

                //終了
                if (stateInfo.normalizedTime >= 1f)
                    skillState = SkillState.Finish;

                break;


            case SkillState.Charge:
                //長押しチェック
                if (inputManager.GetSkillPressed())
                {
                    //チャージ中、フレームごとの処理
                    playerSkillManager.ExecuteSkillChargeFrameBased((PlayerSkill)playerModeManager.Mode);

                    ////今後秒ごとに変化する
                    //statusManager.TakeDamage(0.05f);
                    ////attackSize *= 1.01f;
                    //hitCollider.transform.localScale *= 1.01f;

                    return;
                }

                if (inputManager.GetSkillReleased())
                {
                    skillState = SkillState.ExecuteInit;
                }
                break;


            case SkillState.ExecuteInit:

                //終了処理
                playerSkillManager.ExecuteSkillChargeFinished((PlayerSkill)playerModeManager.Mode);
                skillState = SkillState.Execute;

                break;


            case SkillState.Execute:
                Attack();

                if (animManager.IsAnimEnd)
                {
                    skillState = SkillState.Finish;
                }

                break;


            case SkillState.Finish:
                //アニメーション終了、アイドルへ遷移
                playerController.StateManager.TransitionState(StateType.Idle);

                break;


            default:
                break;
        }
    }

    public override void FixedTick()
    {
        //長押し中移動処理
        if (skillState == SkillState.Charge)
        {
            //移動速度取得
            float moveSpeed = statusManager.StatusData.velocity / 2;

            //方向取得
            Vector2 inputDirec = inputManager.GetMoveDirec();

            rb.velocity = new UnityEngine.Vector3(inputDirec.x * moveSpeed, rb.velocity.y, inputDirec.y * moveSpeed);
        }
    }

    public override void Exit()
    {
        playerController.SpriteRenderer.color = Color.white;

        hitCollider.transform.localScale = defaultAttackSize;
    }

    public void Attack()
    {
        attackAreaPos = transform.position;

        //左右反転か
        offsetPos.x = spriteRenderer.flipX ? Mathf.Abs(offsetPos.x) : -Mathf.Abs(offsetPos.x);

        attackAreaPos += offsetPos;

        //仮処理
        float damage = 0;
        if (statusManager.IsFrenzy) damage = FrenzyAttackDamage;
        else damage = playerSkillManager.SkillData.dicSkill[(PlayerSkill)playerModeManager.Mode].damage;

        //コライダーの検出
        playerController.AttackColliderV2.DetectColliders(damage, enemyLayer, false);

    }

    /*
    /// <summary>
    /// 描画test
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackAreaPos, attackSize);
    }
    */

    #region 引き寄せる
    /*
    private void AttractEnemiesToPlayer()
    {

        float attractRadius = 200f;
        float distanceBetweenEnemies = 1f;


        Collider[] enemies = Physics.OverlapSphere(transform.position, attractRadius, LayerMask.GetMask("Enemy"));
        List<Transform> enemyTransforms = new List<Transform>();

        foreach (Collider enemy in enemies)
        {
            enemyTransforms.Add(enemy.transform);
        }


        Vector3 targetCenter = transform.position + transform.forward * 2f;
        float angleStep = 360f / enemyTransforms.Count;

        for (int i = 0; i < enemyTransforms.Count; i++)
        {

            float angle = i * angleStep;
            Vector3 targetPosition = targetCenter + Quaternion.Euler(0, angle, 0) * Vector3.forward * distanceBetweenEnemies;

            StartCoroutine(MoveEnemyToTarget(enemyTransforms[i], targetPosition));
        }
    }

    private void AttractEnemiesToPlayer()
    {
        float attractRadius = 20f;


        Collider[] enemies = Physics.OverlapSphere(transform.position, attractRadius, LayerMask.GetMask("Enemy"));

        foreach (Collider enemy in enemies)
        {

            Vector3 enemyPosition = enemy.transform.position;


            Vector3 targetPosition = transform.position + playerController.CurDirecVector * 2f;


            StartCoroutine(MoveEnemyToTarget(enemy.transform, targetPosition));
        }
    }

    private IEnumerator MoveEnemyToTarget(Transform enemy, Vector3 targetPosition)
    {
        float duration = 2.0f;
        float elapsed = 0f;

        Vector3 startingPosition = enemy.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            enemy.position = Vector3.Lerp(startingPosition, targetPosition, t);
            yield return null;
        }

        enemy.position = targetPosition;
    }
    */
    #endregion
}

#region 念のためswitch処理を残す
/*
    switch (playerModeManager.Mode)
    {
        case PlayerMode.None:
            playerController.SkillManager.UseSkill(PlayerSkill.Sword);
            break;
        case PlayerMode.Sword:
            playerController.SkillManager.UseSkill(PlayerSkill.Sword);
            break;
        case PlayerMode.Hammer:
            playerController.SkillManager.UseSkill(PlayerSkill.Hammer);
            break;
        case PlayerMode.Spear:
            playerController.SkillManager.UseSkill(PlayerSkill.Spear);
            break;
        case PlayerMode.Gauntlet:
            playerController.SkillManager.UseSkill(PlayerSkill.Gauntlet);
            break;
        default:
            break;
    }
*/
#endregion
