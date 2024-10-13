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

    //PlayerControllerの参照
    PlayerController playerController;

    [SerializeField, Header("ダッシュ持続時間")]
    private float duration = 0.3f;

    [SerializeField, Header("ダッシュ速度")]
    private float speed = 4.0f;

    [SerializeField, Header("ターゲットを貫通可能かどうか")]
    private bool canThrough = false;

    //ダッシュ方向
    UnityEngine.Vector3 direc;

    //ダッシュ中かどうか
    private bool isDashing = false;

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;

        blockDetector.Distance = speed * duration;
    }

    public void Update()
    {
        blockDetector.DetectUpdate(playerController.transform, playerController.InputManager.GetInputDirec());
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            //敵と重ならないため
            //移動先に敵がいなければ、敵との当たり判定を無視する
            if (!blockDetector.IsColliding && canThrough)
            {
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Onomatopoeia"), true);
            }

            blockDetector.IsEnabled = false;
            blockDetector.Distance = speed * duration;

            //一定距離を移動
            playerController.RigidBody.MovePosition(playerController.RigidBody.position + direc.normalized * speed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// 突進処理
    /// </summary>
    /// /// <param name="canThrough"> ターゲットを貫通可能かどうか </param>
    public void Dash(bool _canThrough, Vector3 _direc = default)
    {
        canThrough = _canThrough;

        if (_direc == default)
        {
            //引数なかったら、Lスティックの入力方向を使う
            direc = playerController.InputManager.GetInputDirec();
        }
        else
        {
            direc = _direc;
        }

        //入力をチェック
        if (direc != UnityEngine.Vector3.zero)
        {
            //ダッシュ処理
            StartCoroutine(DashCoroutine());

            Debug.Log("DASH!!!!");//test
            isDashing = true;

        }
    }

    public IEnumerator DashCoroutine()
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {

            yield return null;
        }


        EndDash();
    }

    /// <summary>
    /// 突進終了処理
    /// </summary>
    public void EndDash()
    {
        blockDetector.IsEnabled = true;
        isDashing = false;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Onomatopoeia"), false);

        Debug.Log("DASH END!!!!");
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
        get => this.direc;
        set { this.direc = value; }
    }
}
