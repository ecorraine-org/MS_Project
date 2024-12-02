using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : WorldObjectController
{
    [HideInInspector, Tooltip("エネミーステータスマネージャー")]
    ObjectStatusHandler objectStatus;

    private CapsuleCollider capsuleCollider;

    public override void Awake()
    {
        base.Awake();

        if (!this.transform.GetChild(0).gameObject.TryGetComponent<ObjectStatusHandler>(out objectStatus))
            CustomLogger.LogWarning(objectStatus.GetType(), objectStatus.name);

        BattleManager = BattleManager.Instance;

        capsuleCollider = this.GetComponent<CapsuleCollider>();

        //spawnPool = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject.GetComponent<Collector>();
    }

    void Start()
    {
        //gameObj = Instantiate(Resources.Load<GameObject>(ObjectStatus.StatusData.gameObjPrefab), this.transform);
        gameObj = this.gameObject.transform.parent.gameObject;

        rigidBody.mass = ObjectStatus.StatusData.mass;
        type = ObjectStatus.StatusData.ObjectType;

        EffectHandler = GetComponentInChildren<EffectHandler>();
        EffectHandler.Init(this);

        objState = GetComponentInChildren<ObjectStateHandler>();
        objState.Init(this);
        if (objState == null) Debug.Log("objState NULL");
    }

    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// 攻撃に遷移する際に呼び出す
    /// </summary>
    public void StartAttackCoroutine()
    {
        StartCoroutine(nameof(AttackCoroutine));
    }

    public IEnumerator AttackCoroutine()
    {
        AllowAttack = false;
        yield return new WaitForSeconds(ObjectStatus.ActionCooldown);

        AllowAttack = true;
    }

    public override void Hit(bool _canOneHitKill)
    {
        //被撃状態へ遷移
        isDamaged = true;

        HitReaction hitReaction = BattleManager.GetPlayerHitReaction();

        //エフェクト生成
        EffectHandler.InstantiateHit();
    }

    #region Getter & Setter

    public ObjectStatusHandler ObjectStatus
    {
        get => objectStatus;
    }

    public override Rigidbody RigidBody
    {
        get => this.rigidBody;
    }

    #endregion
}
