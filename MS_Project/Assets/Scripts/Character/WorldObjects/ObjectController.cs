using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>
public abstract class ObjectController : WorldObject
{
    [HideInInspector]
    protected Transform player;

    [ReadOnly, Tooltip("生成されたオブジェクト")]
    public GameObject gameObj;

    [HideInInspector, Tooltip("ステータスマネージャー")]
    ObjectStatusHandler objStatus;

    [HideInInspector, Tooltip("ステートマネージャー")]
    ObjectStateHandler objState;

    [Header("")]
    [SerializeField, Tooltip("破壊できるかどうか？")]
    private bool isInvincible = false;

    [SerializeField, Tooltip("攻撃できるか？")]
    private bool canAttack = true;

    [SerializeField, Tooltip("スキル使用中なのか？")]
    private bool useSkill = false;

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
        if (!this.transform.GetChild(0).gameObject.TryGetComponent<ObjectStatusHandler>(out objStatus))
            CustomLogger.LogWarning(Status.GetType(), Status.name);

        rigidBody = this.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        onomatoObj = Resources.Load<GameObject>("Onomatopoeia/OnomatoItem");
    }

    protected void GenerateOnomatopoeia()
    {
        GameObject collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject;
        onomatoObj.GetComponent<OnomatopoeiaController>().data = Status.StatusData.onomatoData;
        onomatoObj.GetComponent<OnomatopoeiaController>().onomatopoeiaName = Status.StatusData.onomatoData.wordToUse;

        Transform mainCamera = Camera.main.transform;
        // カメラと同じ角度
        Quaternion newRotation = mainCamera.rotation;
        newRotation = newRotation * Quaternion.Euler(0, 0, -90.0f);

        GameObject instance = Instantiate(onomatoObj, this.transform.position, newRotation, collector.transform);
        collector.GetComponent<Collector>().otherObjectPool.Add(instance);
    }

    #region Getter & Setter

    public bool IsInvincible
    {
        get => isInvincible;
        set { isInvincible = value; }
    }

    public bool CanAttack
    {
        get => canAttack;
        set { canAttack = value; }
    }

    public bool UseSkill
    {
        get => useSkill;
        set { useSkill = value; }
    }

    public bool IsDamaged
    {
        get => isDamaged;
        set { isDamaged = value; }
    }

    public ObjectStatusHandler Status
    {
        get => objStatus;
    }

    public ObjectStateHandler State
    {
        get => objState;
        set { objState = value; }
    }

    #endregion
}
