using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DashParam
{
    public float duration; //ダッシュ持続時間
    public float speed;    //ダッシュ速度
    public bool canThrough;//貫通可能かどうか
    public Vector3 dashDirec;     //ダッシュ方向
    public bool isDashing; //ダッシュ中かどうか
    public Coroutine dashCoroutine;//コルーチンの参照
}

/// <summary>
/// 突進処理のビヘイビア
/// </summary>
public class DashHandler : MonoBehaviour
{
    //デバグ用補正終点の位置
    Vector3 testTargetPos;

    [SerializeField, Header("障害物検出ディテクター")]
    BlockDetector blockDetector;

    [SerializeField, Header("プレイヤー周りの障害物検出コライダー")]
    HitCollider hitCollider;

    [SerializeField, Header("攻撃補正用ロックオンコライダー")]
    HitColliderSelected lockOnCollider;

    [SerializeField, Header("フィニッシャー用コライダー")]
    HitColliderKillableEnemy finisherCollider;

    //向かっているターゲットを格納する
    [SerializeField, NonEditable, Header("ターゲットコライダー")]
    Collider targetCollider;

    // シングルトン
    BattleManager battleManager;

    //所有者の参照
    WorldObject owner;

    //コルーチンの参照
    private Coroutine dashCoroutine;

    [SerializeField, Header("ダッシュ持続時間")]
    private float duration = 0.3f;

    //突進開始時間
    private float startTime = 0;

    [SerializeField, Header("ダッシュ速度")]
    private float speed = 4.0f;

    [SerializeField, Header("ヒットストップによる影響")]
    private float slowFactor = 1.0f;

    [SerializeField, Header("ターゲットを貫通可能かどうか")]
    private bool canThrough = false;

    //補正ダッシュ用パラメター
    DashParam correctDashParam;

    //ダッシュ方向
    UnityEngine.Vector3 dashDirec;

    //ダッシュ中かどうか
    private bool isDashing = false;

    //補正とフィニッシャー
    private bool isSpecialDash = false;

    [SerializeField, Header("突進しない距離")]
    float minDashDistance = 2.0f;

    public Vector3 testStart;
    public Vector3 testEnd;
    public float testDIstance;



    public void Init(WorldObject _owner)
    {
        owner = _owner;

        battleManager = BattleManager.Instance;

        if (blockDetector != null) blockDetector.Distance = speed * duration;
    }

    public void Reset()
    {
        speed = 0;
        duration = 0;
        if (blockDetector != null) blockDetector.Reset();
    }

    public void Update()
    {
        //  if (owner is PlayerController)
        // {
        HitReaction hitReaction = battleManager.GetPlayerHitReaction();
        if (battleManager.IsHitStop/*&& hitReaction.stopDuration!=0*/) slowFactor = hitReaction.slowSpeed;
        else slowFactor = 1;
        //   }


        if (blockDetector == null) return;

        if (duration == -1)
        {
            if (owner is PlayerController player)
                blockDetector.Distance = speed * slowFactor * player.AnimManager.testTime;

        }
        else
        {
            blockDetector.Distance = speed * slowFactor * duration;
        }

        //移動しようとする方向
        blockDetector.DetectUpdate(owner.transform, owner.GetNextDirec());

        //#if UNITY_EDITOR
        DebugUpdate();
        //#endif

    }



    private void FixedUpdate()
    {
        if (isDashing)
        {
            //貫通しないパターンで、当たったら終了
            //if (!canThrough && hitCollider != null && hitCollider.CollidersList.Count > 0)
            //{
            //    End();
            //}

            //敵と重ならないため
            //移動先に敵がいなければ、敵との当たり判定を無視する
            if (canThrough)
            {
                if (blockDetector != null && !blockDetector.IsColliding) Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

            }

            //移動中目的地を固定するため、更新停止
            if (blockDetector != null) blockDetector.IsEnabled = false;

            //一定距離を移動
            owner.RigidBody.MovePosition(owner.RigidBody.position + dashDirec.normalized * speed * slowFactor * Time.fixedDeltaTime);

        }


    }

    /// <summary>
    /// 攻撃補正突進処理
    /// </summary>
    public void BeginCorrectDash()
    {
       
        targetCollider = lockOnCollider.ClosestCollider;
        if (lockOnCollider.ClosestCollider == null) return;
        HandleSpecialDash(targetCollider);
    }

    /// <summary>
    /// フィニッシャー突進処理
    /// </summary>
    public void BeginFinishDash()
    {
      
        targetCollider = finisherCollider.KillableCollider;
        if (finisherCollider.KillableCollider == null) return;
        HandleSpecialDash(targetCollider);
    }

    /// <summary>
    /// 攻撃補正とフィニッシャー突進処理
    /// </summary>
    public void HandleSpecialDash(Collider _target)
    {

        if (_target == null)
        {
            return;
        }

        //近すぎると突進しない
        //  if (lockOnCollider.ClosestCollider != null)
        {
            Vector3 ToLock = _target.transform.position - owner.transform.position;
            ToLock.y = 0;
          //  Debug.Log("ToLock.magnitude " + ToLock.magnitude);
            if (ToLock.magnitude < minDashDistance) return;
        }


        Vector3 targetPos = _target.transform.position;

        float offsetX = 3.0f;

        //位置調整
        if ((_target.transform.position - transform.position).x >= 0)
        {
            targetPos.x -= offsetX;
        }
        else
        {
            targetPos.x += offsetX;
        }

        correctDashParam.dashDirec = targetPos - transform.position;
        float distance = (targetPos - transform.position).magnitude;
        //   Debug.Log("TargetDistance " + distance + "!!!!!!!!!!!!!!!!!!!");//test

        testTargetPos = targetPos;

        //水平方向を取得
        correctDashParam.dashDirec.y = 0;


        // 突進初期化
        correctDashParam.speed = 50;
        if (correctDashParam.speed != 0) correctDashParam.duration = distance / correctDashParam.speed; //2;
        else correctDashParam.duration = 0;
        correctDashParam.canThrough = false;

        //FinishTest
        PlayerController player = owner as PlayerController;
        if (player != null && player.StateManager.CurrentStateType == StateType.FinishSkill)
        {
            // 突進初期化
            correctDashParam.speed = 50;
            if (correctDashParam.speed != 0) correctDashParam.duration = distance / correctDashParam.speed; //2;
            else correctDashParam.duration = 0;
            //correctDashParam.duration = 0.1f;
            correctDashParam.canThrough = true;
        }


        startTime = Time.time;

        isSpecialDash = true;

        testStart = owner.transform.position;// test
        //ダッシュ処理
        if (correctDashParam.duration != -1)
            correctDashParam.dashCoroutine = TimerUtility.FrameBasedTimerFixed(this, correctDashParam.duration, () => HandleCorrectDash(targetPos), () => EndCorrectDash());

        correctDashParam.isDashing = true;
    }

    private void HandleCorrectDash(Vector3 _targetPos)
    {

        if (correctDashParam.isDashing)
        {
            //貫通しないパターンで、当たったら終了
            //if (!correctDashParam.canThrough && hitCollider != null && hitCollider.CollidersList.Count > 0)
            //{
            //    EndCorrectDash();
            //}

            //ターゲット位置(z軸)に到着かつ一定距離(平面上)になると終了
            //if (toTargetZ.magnitude < 1.0f /*&& toTargetXZ.magnitude < minDashDistance*/)//1.0f
            //{
            //    EndCorrectDash();
            //}

            Vector3 toTarget = _targetPos - transform.position;

            toTarget.y = 0;
            Vector3 toTargetXZ = toTarget;
            toTarget.x = 0;
            Vector3 toTargetZ = toTarget;

            //FinishTest
            PlayerController player = owner as PlayerController;
            //ターゲットと自分のx座標の比較
            if (targetCollider.transform.position.x - owner.transform.position.x > 0)
            {
                player. SetTwoDirection(true);
              //  Debug.Log("左から右へ");
            }
            else
            {
                player.SetTwoDirection(false);
                //  Debug.Log("右から左へ");
            }

            //敵と重ならないため
            //移動先に敵がいなければ、敵との当たり判定を無視する
            if (correctDashParam.canThrough)
            {
                if (blockDetector != null && !blockDetector.IsColliding) Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

            }

            //移動中目的地を固定するため、更新停止
            if (blockDetector != null) blockDetector.IsEnabled = false;

            //一定距離を移動
           owner.RigidBody.MovePosition(owner.RigidBody.position + correctDashParam.dashDirec.normalized * correctDashParam.speed * slowFactor * Time.fixedDeltaTime);

        }
    }

    /// <summary>
    /// 突進処理
    /// <param name="canThrough"> ターゲットを貫通可能かどうか </param>
    /// </summary>
    public void Begin(bool _canThrough, Vector3 _direc)
    {
        if (isSpecialDash) return;

        canThrough = _canThrough;

        dashDirec = _direc;

        startTime = Time.time;

        //ダッシュ処理
        if (duration != -1)
            dashCoroutine = StartCoroutine(DashCoroutine());

        isDashing = true;
    }

    //canThrough設定なしのバージョン
    public void Begin(Vector3 _direc)
    {
        if (isSpecialDash) return;

        dashDirec = _direc;

        startTime = Time.time;

        //ダッシュ処理
        if (duration != -1)
            dashCoroutine = StartCoroutine(DashCoroutine());

        isDashing = true;
    }

    //距離によって突進しないバージョン(アニメーションイベントから呼び出される)
    public void BeginDashDistanceCheck(Vector3 _direc)
    {
        if (isSpecialDash) return;

        //近すぎると突進しない
        if (lockOnCollider.ClosestCollider != null)
        {
            Vector3 ToLock = lockOnCollider.ClosestCollider.transform.position - owner.transform.position;
            ToLock.y = 0;
            //   Debug.Log("ToLock.magnitude " + ToLock.magnitude);
            if (ToLock.magnitude < minDashDistance) return;
        }

        dashDirec = _direc;

        startTime = Time.time;

        //ダッシュ処理
        if (duration != -1)
            dashCoroutine = StartCoroutine(DashCoroutine());

        isDashing = true;
    }

    public IEnumerator DashCoroutine()
    {
        // 時間累積
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // ヒットストップ時、一旦停止
            if (!battleManager.IsHitStop)
            {
                elapsedTime += Time.deltaTime;
            }

            yield return null;
        }

        End();
    }


    //public IEnumerator DashCoroutine()
    //{
    //    while (Time.time < startTime + duration)
    //    {
    //        yield return null;
    //    }


    //    End();
    //}

    /// <summary>
    /// 突進終了処理
    /// </summary>
    public void End()
    {
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
            dashCoroutine = null;
        }

        if (blockDetector != null) blockDetector.IsEnabled = true;
        isDashing = false;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

    }

    /// <summary>
    /// 突進終了処理
    /// </summary>
    public void EndCorrectDash()
    {
        isSpecialDash = false;

        //testEnd = owner.transform.position;// test
        //testDIstance = (testEnd - testStart).magnitude;
        //Debug.Log("Distance "+testDIstance +"!!!!!!!!!!!!!!!!!!!");

        if (correctDashParam.dashCoroutine != null)
        {
            StopCoroutine(correctDashParam.dashCoroutine);
            correctDashParam.dashCoroutine = null;
        }

        if (blockDetector != null) blockDetector.IsEnabled = true;
        correctDashParam.isDashing = false;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

    }



    public float Duration
    {
        get => this.duration;
        set { this.duration = value; }
    }

    public float Speed
    {
        get => this.speed;
        set { this.speed = value; }
    }

    public bool CanThrough
    {
        get => this.canThrough;
        set { this.canThrough = value; }
    }

    public DashParam CorrectDashParam
    {
        get => this.correctDashParam;
        set { this.correctDashParam = value; }
    }

    public bool IsDashing
    {
        get => this.isDashing;
        set { this.isDashing = value; }
    }

    public Vector3 Direc
    {
        get => this.dashDirec;
        set { this.dashDirec = value; }
    }

    /// <summary>
    /// ロックオンの敵への方向表示
    /// </summary>
    private void OnDrawGizmos()
    {
        PlayerController player = owner as PlayerController;

        if (player == null || lockOnCollider.ClosestCollider == null)
        {
            return;
        }

        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, lockOnCollider.ClosestCollider.transform.position);

        Vector3 targetPos = lockOnCollider.ClosestCollider.transform.position;


        //補正移動終点表示
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(testTargetPos, 0.2f);

    }

    private void DebugUpdate()
    {
        if (lockOnCollider.ClosestCollider == null)
        {
            return;
        }

        Vector3 targetPos = lockOnCollider.ClosestCollider.transform.position;

        float offsetX = 3.0f;

        //位置調整
        if ((lockOnCollider.ClosestCollider.transform.position - transform.position).x >= 0)
        {
            targetPos.x -= offsetX;
        }
        else
        {
            targetPos.x += offsetX;
        }

        //correctDashParam.dashDirec = targetPos - transform.position;

        testTargetPos = targetPos;
    }
}
