using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>
public abstract class WorldObjectController : WorldObject
{
    [HideInInspector]
    protected Transform player;

    protected PlayerController playerController;

    [ReadOnly, Tooltip("生成されたオブジェクト")]
    public GameObject gameObj;

    [HideInInspector, Tooltip("オブジェクトタイプ")]
    protected WorldObjectType type;

    [HideInInspector, Tooltip("ステートマネージャー")]
    protected ObjectStateHandler objState;

    [HideInInspector, Tooltip("エフェクトマネージャー")]
    EffectHandler effectHandler;

    [HideInInspector, Tooltip("コライダーマネジャー")]
    AttackColliderManagerV2 attackColliderManager;

    [Header("簡易ステート")]
    [SerializeField, Tooltip("無敵かどうか？")]
    private bool isInvincible = false;

    [SerializeField, Tooltip("攻撃できるかどうか？")]
    protected bool allowAttack = true;

    [SerializeField, Tooltip("攻撃しているか？")]
    private bool isAttacking = false;

    [SerializeField, Tooltip("スキル使用中なのか？")]
    private bool usingSkill = false;

    [SerializeField, Tooltip("攻撃されているか？")]
    protected bool isDamaged = false;

    [Header("イベント")]
    public UnityEvent OnDamaged;
    public UnityEvent OnAttack;
    public UnityEvent<Vector3> OnMovementInput;
    public Vector3 MovementInput { get; set; }

    protected Rigidbody rigidBody;

    public override void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rigidBody = this.GetComponent<Rigidbody>();

        base.Awake();
    }

    #region Getter & Setter

    public WorldObjectType Type
    {
        get => type;
    }

    public bool IsInvincible
    {
        get => isInvincible;
        set { isInvincible = value; }
    }

    public bool AllowAttack
    {
        get => allowAttack;
        set { allowAttack = value; }
    }

    public bool IsAttacking
    {
        get => isAttacking;
        set { isAttacking = value; }
    }

    public bool UsingSkill
    {
        get => usingSkill;
        set { usingSkill = value; }
    }

    public bool IsDamaged
    {
        get => isDamaged;
        set { isDamaged = value; }
    }

    public ObjectStateHandler State
    {
        get => objState;
        set { objState = value; }
    }

    public EffectHandler EffectHandler
    {
        get => effectHandler;
        set { effectHandler = value; }
    }

    public AttackColliderManagerV2 AttackCollider
    {
        get => attackColliderManager;
        set { attackColliderManager = value; }
    }

    public PlayerController PlayerController
    {
        get => playerController;
      //  set { playerController = value; }
    }

    #endregion
}
