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

    Rigidbody thisRigidbody;

    // UnityEngine.Vector2 moveInput;
    //public float moveSpeed = 1;
    public float jumpForce = 3;

    public LayerMask terrainLayer;
    public Transform groundCheck;
    public bool isGrounded;

    // 
    private Transform sprite;
    private SpriteRenderer spriteRenderer;
    private Animator spriteAnim;
    private Animator playerFlip;

    // 
    private Transform mainCamera;

    //入力方向
    UnityEngine.Vector2 inputDirec;

    //現在の向き
    Direction currentDirec = Direction.Down;

    //ダメージ受けるかどうか
    bool isHit;

    // 方向角度の閾値
    private const float angleThreshold = 22.5f;

    //攻撃test
    public UnityEngine.Vector3 attackSize = new UnityEngine.Vector3(1f, 1f, 1f);
    UnityEngine.Vector3 attackAreaPos;
    public UnityEngine.Vector3 offsetPos;
    public float attackDamage;
    public LayerMask enemyLayer;

    void Awake()
    {
        inputManager = PlayerInputManager.Instance;

        thisRigidbody = GetComponent<Rigidbody>();

        mainCamera = GameObject.FindWithTag("MainCamera").gameObject.transform;

        terrainLayer = LayerMask.GetMask("Terrain");

        playerFlip = GetComponent<Animator>();

        sprite = gameObject.transform.GetChild(0);
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        spriteAnim = sprite.GetComponent<Animator>();

        //依存性注入
        stateManager.Init(this);
        animManager.Init(this);
    }

    void Start()
    {

        groundCheck = gameObject.transform.GetChild(1).gameObject.transform;
        if (Debug.isDebugBuild)
            Debug.Log(gameObject.transform.GetChild(1).gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        //moveInput.x = Input.GetAxis("Horizontal");
        //moveInput.y = Input.GetAxis("Vertical");
        //moveInput.Normalize();
        //if (Debug.isDebugBuild)
        //    Debug.Log("Movement: " + moveInput);


        //spriteAnim.SetFloat("MoveSpeed", thisRigidbody.velocity.magnitude);

        //if (thisRigidbody.velocity.magnitude > 0)
        //{
        //    spriteRenderer.flipX = false;
        //    if (CurrentDirection() == Direction.Up)
        //    {
        //        spriteAnim.Play("WalkUp");
        //    }
        //    else if (CurrentDirection() == Direction.Down)
        //    {
        //        spriteAnim.Play("WalkDown");
        //    }
        //    else if (CurrentDirection() == Direction.Left)
        //    {
        //        spriteRenderer.flipX = true;
        //        spriteAnim.Play("WalkRight");
        //    }
        //    else if (CurrentDirection() == Direction.Right)
        //    {
        //        spriteAnim.Play("WalkRight");
        //    }
        //    else if (CurrentDirection() == Direction.UpLeft)
        //    {
        //        spriteRenderer.flipX = true;
        //        spriteAnim.Play("WalkUpRight");
        //    }
        //    else if (CurrentDirection() == Direction.UpRight)
        //    {
        //        spriteAnim.Play("WalkUpRight");
        //    }
        //    else if (CurrentDirection() == Direction.DownLeft)
        //    {
        //        spriteRenderer.flipX = true;
        //        spriteAnim.Play("WalkDownRight");
        //    }
        //    else if (CurrentDirection() == Direction.DownRight)
        //    {
        //        spriteAnim.Play("WalkDownRight");
        //    }
        //}

        //transform.forward = mainCamera.forward;
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

    //attack test
    public void Attack()
    {
        attackAreaPos = transform.position;

        //左右反転か
        offsetPos.x = spriteRenderer.flipX ? -Mathf.Abs(offsetPos.x) : Mathf.Abs(offsetPos.x);

        attackAreaPos += offsetPos;

        Debug.Log("controller Attack");
        Collider[] hitColliders = Physics.OverlapBox(attackAreaPos, attackSize / 2, UnityEngine.Quaternion.identity, enemyLayer);
        Debug.Log("当たり判定" + hitColliders[0]);//test
        foreach (Collider hitCollider in hitColliders)
        {
            hitCollider.GetComponentInChildren<ILife>().TakeDamage(attackDamage);
        }
    }

    /// <summary>
    /// 描画test
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackAreaPos, attackSize);
    }

    /// <summary>
    /// 攻撃イベントtest
    /// </summary>
    private void OnAttackPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        spriteAnim.Play("PlayerAttack");
    }

    /// <summary>
    /// 入力方向で現在の方向を八方向のいずれかに設定する
    /// </summary>
    public void SetEightDirection()
    {
        UnityEngine.Vector2 inputDirec = inputManager.GetMoveDirec();

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

    public void SetWalkAnimation()
    {
        spriteAnim.SetFloat("MoveSpeed", thisRigidbody.velocity.magnitude);

        spriteRenderer.flipX = false;

        switch (currentDirec)
        {
            case Direction.Right:
                spriteAnim.Play("WalkRight");
                break;

            case Direction.UpRight:
                spriteAnim.Play("WalkUpRight");
                break;

            case Direction.Up:
                spriteAnim.Play("WalkUp");
                break;

            case Direction.UpLeft:
                spriteRenderer.flipX = true;
                spriteAnim.Play("WalkUpRight");
                break;

            case Direction.Left:
                spriteRenderer.flipX = true;
                spriteAnim.Play("WalkRight");
                break;

            case Direction.DownLeft:
                spriteRenderer.flipX = true;
                spriteAnim.Play("WalkDownRight");
                break;

            case Direction.Down:
                spriteAnim.Play("WalkDown");
                break;

            case Direction.DownRight:
                spriteAnim.Play("WalkDownRight");
                break;
        }
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
