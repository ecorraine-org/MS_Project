using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // インプットシングルトン
    protected PlayerInputManager inputManager;

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

    private Collider playerCcollider;

    [SerializeField, Header("アタックコライダーマネージャー")]
    AttackColliderManager attackCollider;

    Rigidbody thisRigidbody;

   // public float jumpForce = 3;

    public LayerMask terrainLayer;
    public Transform groundCheck;
    public bool isGrounded;

    // 
    private Transform sprite;
    private SpriteRenderer spriteRenderer;
    [HideInInspector]public Material material;
    private Animator spriteAnim;
    private Animator playerFlip;

    // 
    private Transform mainCamera;

    //入力方向
    UnityEngine.Vector2 inputDirec;

    //現在の向き
    Direction currentDirec = Direction.Down;

    //現在の向き(ベクトル)
    UnityEngine.Vector3 curDirecVector;


    //ダメージ受けるかどうか
    bool isHit;

    // 入力方向角度の閾値
    private const float angleThreshold = 22.5f;

    void Awake()
    {
        inputManager = PlayerInputManager.Instance;

        thisRigidbody = GetComponent<Rigidbody>();

        mainCamera = GameObject.FindWithTag("MainCamera").gameObject.transform;

        terrainLayer = LayerMask.GetMask("Terrain");

        playerFlip = GetComponent<Animator>();

        playerCcollider = GetComponent<Collider>();

        sprite = gameObject.transform.GetChild(0);
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
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
    }

    void Start()
    {

        groundCheck = gameObject.transform.GetChild(1).gameObject.transform;
        if (Debug.isDebugBuild)
            Debug.Log(gameObject.transform.GetChild(1).gameObject.name);
    }

    private void FixedUpdate()
    {
        //thisRigidbody.velocity = new UnityEngine.Vector3(moveInput.x * moveSpeed, thisRigidbody.velocity.y, moveInput.y * moveSpeed);

        //RaycastHit raycastHit;
        //if (Physics.Raycast(groundCheck.position, UnityEngine.Vector3.down, out raycastHit, 0.1f, terrainLayer))
        //{
        //    isGrounded = true;
        //}
        //else
        //{
        //    isGrounded = false;
        //}
        //Debug.DrawRay(groundCheck.position, UnityEngine.Vector2.down, Color.red);

        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    //canJump = true;
        //    //thisRigidbody.AddForce(UnityEngine.Vector3.up * jumpForce, ForceMode.Impulse);
        //    thisRigidbody.velocity += new UnityEngine.Vector3(0f, jumpForce, 0f);
        //}
    }

  

   

    /// <summary>
    /// 入力方向で現在の方向を八方向のいずれかに設定する
    /// </summary>
    public void SetEightDirection()
    {
        UnityEngine.Vector2 inputDirec = inputManager.GetMoveDirec();

        curDirecVector = inputManager.GetInputDirec();

        //移動しない時、向きを保つため
        if (inputDirec == UnityEngine.Vector2.zero) return;

        //入力方向の角度計算
        float angle = Mathf.Atan2(inputDirec.y, inputDirec.x) * Mathf.Rad2Deg;
        if (angle >= -angleThreshold && angle < angleThreshold)
        {
            currentDirec = Direction.Right;
        }

        if (angle >= angleThreshold && angle < angleThreshold * 3)
        {

            currentDirec = Direction.UpRight;
        }
        if (angle >= angleThreshold * 3 && angle < angleThreshold * 5)
        {

            currentDirec = Direction.Up;
        }

        if (angle >= angleThreshold * 5 && angle < angleThreshold * 7)
        {
            currentDirec = Direction.UpLeft;
        }

        if (angle >= angleThreshold * 7 || angle < -angleThreshold * 7)
        {
            currentDirec = Direction.Left;
        }

        if (angle >= -angleThreshold * 7 && angle < -angleThreshold * 5)
        {
            currentDirec = Direction.DownLeft;
        }

        if (angle >= -angleThreshold * 5 && angle < -angleThreshold * 3)
        {
            currentDirec = Direction.Down;
        }

        if (angle >= -angleThreshold * 3 && angle < -angleThreshold)
        {
            currentDirec = Direction.DownRight;
        }
    }

    /// <summary>
    /// 現在の方向で歩きのアニメーションを設定する
    /// </summary>
    public void SetWalkAnimation()
    {
        spriteAnim.SetFloat("MoveSpeed", thisRigidbody.velocity.magnitude);

        spriteRenderer.flipX = false;

        switch (currentDirec)
        {
            case Direction.Right:
                spriteRenderer.flipX = true;
                spriteAnim.Play("WalkRight");
                break;

            case Direction.UpRight:
                spriteRenderer.flipX = true;
                spriteAnim.Play("WalkUpRight");
                break;

            case Direction.Up:
                spriteAnim.Play("WalkUp");
                break;

            case Direction.UpLeft:

                spriteAnim.Play("WalkUpRight");
                break;

            case Direction.Left:
                spriteAnim.Play("WalkRight");
                break;

            case Direction.DownLeft:
                spriteAnim.Play("WalkDownRight");
                break;

            case Direction.Down:
                spriteAnim.Play("WalkDown");
                break;

            case Direction.DownRight:
                spriteRenderer.flipX = true;
                spriteAnim.Play("WalkDownRight");
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + curDirecVector);
    }

    #region Getter&Setter 

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

    public AttackColliderManager AttackCollider
    {
        get => this.attackCollider;
    }

    public PlayerModeManager ModeManager
    {
        get => this.modeManager;
    }

    public Animator SpriteAnim
    {
        get => this.spriteAnim;
    }

    public Transform MainCamera
    {
        get => this.mainCamera;
    }

    public SpriteRenderer SpriteRenderer
    {
        get => this.spriteRenderer;
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

    public Rigidbody RigidBody
    {
        get => this.thisRigidbody;
    }



    #endregion
}
