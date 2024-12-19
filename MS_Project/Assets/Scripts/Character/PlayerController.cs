using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : WorldObject
{
    public TutorialStage tutorialStage= TutorialStage.None;

    [SerializeField, Header("足元のエフェクト")]
    GameObject dustEffect;
    //格納する
    GameObject dustEffectInstance;

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

    [SerializeField, Header("オーディオマネージャー")]
    AudioManager audioManager;

    [SerializeField, Header("バフマネージャー")]
    PlayerBuffManager buffManager;

    private Collider playerCollider;

    [SerializeField, Header("アタックコライダーマネージャー")]
    AttackColliderManagerV2 attackColliderV2;

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
    Direction preDirec= Direction.None;

    //現在の向き(ベクトル)
    UnityEngine.Vector3 curDirecVector = new UnityEngine.Vector3(-1, 0, 0);

    // 入力方向角度の閾値
    private const float angleThreshold = 22.5f;

    [SerializeField, Header("死亡時のフェード用パネル")]
    public Image fadePanel;             // フェード用のUIパネル（Image）
    public float fadeDuration = 1.0f;   // フェードの完了にかかる時間
    [SerializeField, Header("遷移先のシーン名")]
    string sceneToLoad; // 切り替えるシーン名を指定

    public override void Awake()
    {
        base.Awake();

        //入力設定
        InputController.Instance.SetInputContext(InputController.InputContext.Player);

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
        buffManager.Init(this);
    }

    void Start()
    {


        groundCheck = gameObject.transform.GetChild(1).gameObject.transform;
        if (Debug.isDebugBuild)
            Debug.Log(gameObject.transform.GetChild(1).gameObject.name);

        fadePanel.enabled = false;       // フェードパネルを無効化
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0.0f); // 初期状態では透明


        //dustEffectInstance = Instantiate(dustEffect, transform.position, UnityEngine.Quaternion.LookRotation(-transform.forward) * UnityEngine.Quaternion.Euler(60, 0, 0), transform);
    }



    private void Update()
    {
        GenerateDustEffect();

        //仮設定
        BattleManager.Instance.CurPlayerMode =modeManager.Mode;


        //デバグ用即死
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha2))
        {
            statusManager.CurrentHealth = 0;
        }

        if(statusManager.CurrentHealth <= 0)
        {
            StartCoroutine(FadeOutAndLoadScene());
        }

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
    /// 左右向きを設定する
    /// </summary>
    public void SetTwoDirection(bool _isRight)
    {
        if (_isRight)
        {
            spriteRenderer.flipX = true;
            curDirecVector = new UnityEngine.Vector3(1, 0, 0);
        }
        else
        {
            spriteRenderer.flipX = false;
            curDirecVector = new UnityEngine.Vector3(-1, 0, 0);
        }
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
            case Direction.UpRight:
                spriteAnim.Play("WalkUpRight");
                break;
            case Direction.UpLeft:

                spriteAnim.Play("WalkUpRight");
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
        if (statusManager.IsInvincible) return;
        //被撃状態へ遷移
        statusManager.IsHit = true;
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

    public override void GenerateOnomatopoeia(GameObject _owner, OnomatopoeiaData _onomatopoeiaData) { }

    private void GenerateDustEffect()
    {
        if (preDirec != currentDirec)
        {

            if (dustEffectInstance != null)
            {
                dustEffectInstance.transform.SetParent(null);
                ParticleSystem particleSystem = dustEffectInstance.GetComponent<ParticleSystem>();

                if (particleSystem != null)
                {
                    // ループ停止
                    var mainModule = particleSystem.main;
                    mainModule.loop = false;
                }
            }
            //消滅してから、新しいのを生成
            if (stateManager.CurrentStateType == StateType.Walk || stateManager.CurrentStateType == StateType.Dodge)
                dustEffectInstance = Instantiate(dustEffect, transform.position, UnityEngine.Quaternion.LookRotation(-transform.forward) * UnityEngine.Quaternion.Euler(60, 0, 0), transform);

        }
        if (stateManager.CurrentStateType == StateType.Walk || stateManager.CurrentStateType == StateType.Dodge)
            preDirec = currentDirec;

        //消す
        if (stateManager.CurrentStateType != StateType.Walk && stateManager.CurrentStateType != StateType.Dodge)
        {
            if (dustEffectInstance != null)
            {
                dustEffectInstance.transform.SetParent(null);
                ParticleSystem particleSystem = dustEffectInstance.GetComponent<ParticleSystem>();

                if (particleSystem != null)
                {
                    // ループ停止
                    var mainModule = particleSystem.main;
                    mainModule.loop = false;
                }
            }
        }




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

    //public bool IsHit
    //{
    //    get => this.isHit;
    //    set { this.isHit = value; }
    //}

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

    public AudioManager AudioManager
    {
        get => this.audioManager;
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

    public PlayerBuffManager BuffManager
    {
        get => this.buffManager;
    }

    public override Rigidbody RigidBody
    {
        get => thisRigidbody;
    }

    #endregion

     public IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;   // フェードパネルを有効化

        float elapsedTime = 0.0f;                        // 経過時間を初期化
        Color startColor = fadePanel.color;              // フェードパネルの開始色を取得
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定

        // フェードアウトアニメーションを実行
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // 経過時間を増やす
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
            yield return null;                                     // 1フレーム待機
        }

        fadePanel.color = endColor;                                // フェードが完了したら最終色に設定
        SceneManager.LoadScene(sceneToLoad);                    // シーンをロードしてメニューシーンに遷移
    }
}


//private void TutorialUpdate()
//{
//    //仮でチュートリアル
//    switch (tutorialStage)
//    {
//        //case TutorialStage.Step1:
//        //    Debug.Log("チュートリアル第1段階");


//        //    break;

//        //時間速度を戻す
//        case TutorialStage.Step2:
//            Debug.Log("チュートリアル第2段階");

//            InputController.Instance.SetInputContext(InputController.InputContext.Player);
//            Time.timeScale = 1;

//            break;
//        case TutorialStage.Step3:
//            if (Input.GetKey(KeyCode.Return))
//            {
//                InputController.Instance.SetInputContext(InputController.InputContext.Player);
//                Time.timeScale = 1;
//            }

//            break;
//        case TutorialStage.None:
//            break;
//    }
//}
