using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : WorldObject
{
    //シングルトン
    protected PlayerInputManager inputManager;
    BattleManager battleManager;

    [SerializeField, Header("スプライトオブジェクト")]
    GameObject spriteObject;

    [SerializeField, Header("ステータスマネージャー")]
    PlayerStatusManager statusManager;

    [SerializeField, Header("ステートマネージャー")]
    PlayerStateManager stateManager;

    [SerializeField, Header("アニメーションマネージャー")]
    PlayerAnimManager animManager;

    [SerializeField, Header("スキルマネージャー")]
    PlayerSkillManager skillManager;

    [SerializeField, Header("モードマネージャー")]
    PlayerModeManager modeManager;

    private Collider playerCollider;

    [SerializeField, Header("アタックコライダーマネージャー")]
    AttackColliderManagerV2 attackColliderV2;

    //[SerializeField, Header("ヒットコライダー")]
    //HitCollider hitCollider;

    [SerializeField, Header("エネミーディテクター")]
    DetectEnemyArea detectEnemy;

    Rigidbody thisRigidbody;

    public LayerMask terrainLayer;
    public Transform groundCheck;
    public bool isGrounded;

    // 
    private Transform sprite;
    private SpriteRenderer spriteRenderer;
    [HideInInspector] public Material material;
    private Animator spriteAnim;
    private Animator playerFlip;

    //入力方向
    UnityEngine.Vector2 inputDirec;

    //現在の向き
    Direction currentDirec = Direction.Left;

    //現在の向き(ベクトル)
    UnityEngine.Vector3 curDirecVector = new UnityEngine.Vector3(-1, 0, 0);

    //ダメージ受けるかどうか
    bool isHit;

    // 入力方向角度の閾値
    private const float angleThreshold = 22.5f;

    public override void Awake()
    {
        base.Awake();

        inputManager = PlayerInputManager.Instance;
        battleManager = BattleManager.Instance;

        thisRigidbody = GetComponent<Rigidbody>();

        terrainLayer = LayerMask.GetMask("Terrain");

        playerFlip = GetComponent<Animator>();

        playerCollider = GetComponent<Collider>();

        sprite = gameObject.transform.GetChild(0);
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = false;
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        material = spriteRenderer.material;

        spriteAnim = sprite.GetComponent<Animator>();

        //依存性注入
        stateManager.Init(this);
        animManager.Init(this);
        modeManager.Init(this);
        skillManager.Init(this);
        statusManager.Init(this);
        inputManager.Init(this);
        detectEnemy.Init(this);
    }

    void Start()
    {
     

        groundCheck = gameObject.transform.GetChild(1).gameObject.transform;
        if (Debug.isDebugBuild)
            Debug.Log(gameObject.transform.GetChild(1).gameObject.name);
    }

    private void Update()
    {
        //前方向で向き設定
       UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(GetForward());
        transform.rotation = UnityEngine.Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

        //スプライトをカメラに向く
        spriteObject.transform.rotation = Camera.main.transform.rotation;
    }

    private void FixedUpdate()
    {
        UnityEngine.Vector3 gravity = Physics.gravity * (RigidBody.mass * RigidBody.mass);
        RigidBody.AddForce(gravity * Time.deltaTime);

        //sprite.transform.rotation = Camera.main.transform.rotation;
        /*
        Debug.Log("CurDirec "+curDirecVector);

        thisRigidbody.velocity = new UnityEngine.Vector3(moveInput.x * moveSpeed, thisRigidbody.velocity.y, moveInput.y * moveSpeed);

        RaycastHit raycastHit;
        if (Physics.Raycast(groundCheck.position, UnityEngine.Vector3.down, out raycastHit, 0.1f, terrainLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        Debug.DrawRay(groundCheck.position, UnityEngine.Vector2.down, Color.red);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //canJump = true;
            //thisRigidbody.AddForce(UnityEngine.Vector3.up * jumpForce, ForceMode.Impulse);
            thisRigidbody.velocity += new UnityEngine.Vector3(0f, jumpForce, 0f);
        }
         */
    }

    /// <summary>
    /// 入力方向で現在の方向を八方向のいずれかに設定する
    /// </summary>
    public void SetEightDirection()
    {
        UnityEngine.Vector2 inputDirec = inputManager.GetMoveDirec();

        //移動しない時、向きを保つため
        if (inputDirec == UnityEngine.Vector2.zero) return;

        curDirecVector = inputManager.GetLStick().normalized;

        //入力方向の角度計算
        float angle = Mathf.Atan2(inputDirec.y, inputDirec.x) * Mathf.Rad2Deg;
        if (angle >= -angleThreshold && angle < angleThreshold)
        {
            spriteRenderer.flipX = true;
            currentDirec = Direction.Right;
        }

        if (angle >= angleThreshold && angle < angleThreshold * 3)
        {
            spriteRenderer.flipX = true;
            currentDirec = Direction.UpRight;
        }
        if (angle >= angleThreshold * 3 && angle < angleThreshold * 5)
        {

            currentDirec = Direction.Up;
        }

        if (angle >= angleThreshold * 5 && angle < angleThreshold * 7)
        {
            spriteRenderer.flipX = false;
            currentDirec = Direction.UpLeft;
        }

        if (angle >= angleThreshold * 7 || angle < -angleThreshold * 7)
        {
            spriteRenderer.flipX = false;
            currentDirec = Direction.Left;
        }

        if (angle >= -angleThreshold * 7 && angle < -angleThreshold * 5)
        {
            spriteRenderer.flipX = false;
            currentDirec = Direction.DownLeft;
        }

        if (angle >= -angleThreshold * 5 && angle < -angleThreshold * 3)
        {
            currentDirec = Direction.Down;
        }

        if (angle >= -angleThreshold * 3 && angle < -angleThreshold)
        {
            spriteRenderer.flipX = true;
            currentDirec = Direction.DownRight;
        }
    }

    /// <summary>
    /// 現在の方向で歩きのアニメーションを設定する
    /// </summary>
    public void SetWalkAnimation()
    {
        // spriteAnim.SetFloat("MoveSpeed", thisRigidbody.velocity.magnitude);

       

        switch (currentDirec)
        {
            case Direction.Up:
                   spriteAnim.Play("WalkUp");
                    break;
            default:
                spriteAnim.Play("WalkRight");
                break;
        }

        //switch (currentDirec)
        //{
        //    case Direction.Right:
        //        spriteAnim.Play("WalkRight");
        //        break;

        //    case Direction.UpRight:
        //        spriteAnim.Play("WalkUpRight");
        //        break;

        //    case Direction.Up:
        //        spriteAnim.Play("WalkUp");
        //        break;

        //    case Direction.UpLeft:

        //        spriteAnim.Play("WalkUpRight");
        //        break;

        //    case Direction.Left:
        //        spriteAnim.Play("WalkRight");
        //        break;

        //    case Direction.DownLeft:
        //        spriteAnim.Play("WalkDownRight");
        //        break;

        //    case Direction.Down:
        //        spriteAnim.Play("WalkDown");
        //        break;

        //    case Direction.DownRight:
        //        spriteAnim.Play("WalkDownRight");
        //        break;
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

      //  Gizmos.DrawLine(transform.position, transform.position + GetForward());

        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }

    public override void Hit(bool _canOneHitKill)
    {

    }

    public override void Attack(Collider _hitCollider)
    {
    
        if (_hitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy")
            || _hitCollider.gameObject.layer == LayerMask.NameToLayer("Onomatopoeia"))
        {
            // ヒットストップ          
            battleManager.StartHitStop(spriteAnim);
        }
    }

    public override void Miss()
    {
        base.Miss();

       // Debug.Log("空振り処理");
      //  GenerateOnomatopoeia(statusManager.StatusData.onomato???);
    }

    #region Getter&Setter 

    /// <summary>
    /// 移動しようとする方向(Lスティック方向)を取得
    /// </summary>
    public override UnityEngine.Vector3 GetNextDirec()
    {
        if (inputManager.GetLStick() != UnityEngine.Vector3.zero)
        {
            return inputManager.GetLStick().normalized;
        }
        //入力がない場合、前方向を返す
        else
        {
            return base.GetNextDirec();
        }

      
    }

    /// <summary>
    /// 前方向を取得
    /// </summary>
    public override UnityEngine.Vector3 GetForward()
    {
        
        if (stateManager.CurrentStateType == StateType.Walk
            || stateManager.CurrentStateType == StateType.Idle
             || stateManager.CurrentStateType == StateType.Dodge)
        {
            return curDirecVector;
        }
        //移動とアイドルではない時、左右方向だけ
        else
        {
            if (spriteRenderer.flipX == true)
                return new UnityEngine.Vector3(1, 0, 0);

            if (spriteRenderer.flipX == false)
                return new UnityEngine.Vector3(-1, 0, 0);
        }

        //エラーの場合
        return new UnityEngine.Vector3(0, 1, 0);
    }

    public bool IsHit
    {
        get => this.isHit;
        set { this.isHit = value; }
    }

    public PlayerStateManager StateManager
    {
        get => this.stateManager;
    }

    public PlayerSkillManager SkillManager
    {
        get => this.skillManager;
    }

    public AttackColliderManagerV2 AttackColliderV2
    {
        get => this.attackColliderV2;
    }

    public PlayerModeManager ModeManager
    {
        get => this.modeManager;
    }

    public PlayerAnimManager AnimManager
    {
        get => this.animManager;
    }


    public PlayerInputManager InputManager
    {
        get => this.inputManager;
    }

    public BattleManager BattleManager
    {
        get => this.battleManager;
    }

    public Animator SpriteAnim
    {
        get => this.spriteAnim;
    }

    public SpriteRenderer SpriteRenderer
    {
        get => this.spriteRenderer;
    }

    public DetectEnemyArea DetectEnemy
    {
        get => this.detectEnemy;
    }

    public Direction CurrentDirec
    {
        get => this.currentDirec;
        set { this.currentDirec = value; }
    }

    public UnityEngine.Vector3 CurDirecVector
    {
        get => this.curDirecVector;
        set { this.curDirecVector = value; }
    }

    public PlayerStatusManager StatusManager
    {
        get => this.statusManager;
    }

    public override Rigidbody RigidBody
    {
        get => thisRigidbody;
    }

    #endregion
}
