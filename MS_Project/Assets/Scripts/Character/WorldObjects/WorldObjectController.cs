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

    [ReadOnly, Tooltip("生成されたオブジェクト")]
    public GameObject gameObj;

    [HideInInspector, Tooltip("オブジェクトタイプ")]
    protected WorldObjectType type;

    [HideInInspector, Tooltip("ステートマネージャー")]
    ObjectStateHandler objState;

    [HideInInspector, Tooltip("エフェクトマネージャー")]
    EffectHandler effectHandler;

    [HideInInspector, Tooltip("コライダーマネジャー")]
    AttackColliderManagerV2 attackColliderManager;

    [Header("簡易ステート")]
    [SerializeField, Tooltip("無敵かどうか？")]
    private bool isInvincible = false;

    [SerializeField, Tooltip("攻撃できるかどうか？")]
    private bool allowAttack = false;

    [SerializeField, Tooltip("攻撃しているか？")]
    private bool isAttacking = false;

    [SerializeField, Tooltip("スキル使用中なのか？")]
    private bool usingSkill = false;

    [SerializeField, Tooltip("攻撃されているか？")]
    private bool isDamaged = false;

    [Header("イベント")]
    public UnityEvent OnDamaged;
    public UnityEvent OnAttack;
    public UnityEvent<Vector3> OnMovementInput;
    public Vector3 MovementInput { get; set; }

    protected Rigidbody rigidBody;

    [HideInInspector, Tooltip("生成するオノマトペオブジェクト")]
    protected GameObject onomatoObj;

    public virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidBody = this.GetComponent<Rigidbody>();

        onomatoObj = Resources.Load<GameObject>("Onomatopoeia/OnomatoItem");
    }

    public void GenerateOnomatopoeia(OnomatopoeiaData _onomatopoeiaData)
    {
        GameObject collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject;
        onomatoObj.GetComponent<OnomatopoeiaController>().data = _onomatopoeiaData;
        onomatoObj.GetComponent<OnomatopoeiaController>().onomatopoeiaName = _onomatopoeiaData.wordToUse;

        Transform mainCamera = Camera.main.transform;
        // カメラと同じ角度
        Quaternion newRotation = mainCamera.rotation;
        newRotation = newRotation * Quaternion.Euler(0, 0, -90.0f);

        GameObject instance = Instantiate(onomatoObj, this.transform.position, newRotation, collector.transform);
        collector.GetComponent<Collector>().otherObjectPool.Add(instance);
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

    #endregion
}
