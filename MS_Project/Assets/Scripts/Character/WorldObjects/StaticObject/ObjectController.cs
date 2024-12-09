using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : WorldObjectController
{
    [HideInInspector, Tooltip("エネミーステータスマネージャー")]
    ObjectStatusHandler objectStatus;

    [HideInInspector, Tooltip("ステートマネージャー")]
    ObjectStateHandler objState;

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

        rigidBody.mass = Status.StatusData.mass;
        type = Status.StatusData.ObjectType;

        EffectHandler = GetComponentInChildren<EffectHandler>();
        EffectHandler.Init(this);

        objState = GetComponentInChildren<ObjectStateHandler>();
        if (objState == null) Debug.Log("objState NULL");
        objState.Init(this);
    }

    private void Update()
    {
        if (onomatoPool.Count < maxOnomatopoeiaCount)
        {
            canGenerateOnomatopoeia = true;
        }
        else
        {
            canGenerateOnomatopoeia = false;
        }
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
        yield return new WaitForSeconds(Status.ActionCooldown);

        AllowAttack = true;
    }

    public override void Hit(bool _canOneHitKill)
    {
        //被撃状態へ遷移
        isDamaged = true;

        //エフェクト生成
        EffectHandler.InstantiateHit();
    }

    public override void GenerateOnomatopoeia(GameObject _owner, OnomatopoeiaData _onomatopoeiaData)
    {
        if (canGenerateOnomatopoeia)
        {
            GameObject collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject;

            onomatoObj.GetComponent<OnomatopoeiaController>().OwningObject = _owner;
            onomatoObj.GetComponent<OnomatopoeiaController>().Data = _onomatopoeiaData;
            onomatoObj.GetComponent<OnomatopoeiaController>().onomatopoeiaName = _onomatopoeiaData.wordToUse;

            Transform mainCamera = Camera.main.transform;
            //カメラと同じ角度にする
            Quaternion newRotation = mainCamera.rotation;
            //newRotation = newRotation * Quaternion.Euler(0, 0, -90.0f);

            Vector3 newPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - GetComponent<Collider>().bounds.extents.z);

            GameObject instance = Instantiate(onomatoObj, newPosition, newRotation, collector.transform);
            onomatoPool.Add(instance);
            collector.GetComponent<ObjectCollector>().otherObjectPool.Add(instance);
        }
    }

    #region Getter & Setter

    public ObjectStatusHandler Status
    {
        get => objectStatus;
    }

    public ObjectStateHandler State
    {
        get => objState;
    }

    public override Rigidbody RigidBody
    {
        get => this.rigidBody;
    }

    #endregion
}
