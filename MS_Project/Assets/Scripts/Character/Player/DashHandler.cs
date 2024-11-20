using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 突進処理のビヘイビア
/// </summary>
public class DashHandler : MonoBehaviour
{
    [SerializeField, Header("障害物検出ディテクター")]
    BlockDetector blockDetector;

    [SerializeField, Header("プレイヤー周りの障害物検出コライダー")]
    HitCollider hitCollider;

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

    //ダッシュ方向
    UnityEngine.Vector3 dashDirec;

    //ダッシュ中かどうか
    private bool isDashing = false;

    public void Init(WorldObject _owner)
    {
        owner = _owner;

        battleManager = BattleManager.Instance;

        if (blockDetector!=null) blockDetector.Distance = speed * duration;
    }

    public void Reset()
    {
        speed = 0;
        duration = 0;
        if (blockDetector != null) blockDetector.Reset();
    }

    public void Update()
    {
        HitReaction hitReaction = battleManager.GetPlayerHitReaction();
        if (battleManager.IsHitStop/*&& hitReaction.stopDuration!=0*/) slowFactor = hitReaction.slowSpeed;
        else slowFactor = 1;

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
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            //貫通しないパターンで、当たったら終了
            if (!canThrough && hitCollider!=null && hitCollider.CollidersList.Count > 0)
            {
                End();
            }       

            //敵と重ならないため
            //移動先に敵がいなければ、敵との当たり判定を無視する
            if ( canThrough )
            {
                if(blockDetector != null&&!blockDetector.IsColliding) Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

            }

            //移動中目的地を固定するため、更新停止
            if(blockDetector != null)  blockDetector.IsEnabled = false;

            //一定距離を移動
            owner.RigidBody.MovePosition(owner.RigidBody.position + dashDirec.normalized * speed * slowFactor * Time.fixedDeltaTime);

          
        }
    }

    /// <summary>
    /// 突進処理
    /// <param name="canThrough"> ターゲットを貫通可能かどうか </param>
    /// </summary>
    public void Begin(bool _canThrough, Vector3 _direc)
    {

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
        dashDirec = _direc;

        startTime = Time.time;

        //ダッシュ処理
        if (duration != -1)
            dashCoroutine = StartCoroutine(DashCoroutine());

        isDashing = true;
    }

    public IEnumerator DashCoroutine()
    {
        while (Time.time < startTime + duration)
        {
            yield return null;
        }


        End();
    }

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
}
