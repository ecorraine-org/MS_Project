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

        capsuleCollider = this.GetComponent<CapsuleCollider>();

        //spawnPool = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject.GetComponent<Collector>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //gameObj = Instantiate(Resources.Load<GameObject>(ObjectStatus.StatusData.gameObjPrefab), this.transform);

        rigidBody.mass = ObjectStatus.StatusData.mass;
        type = ObjectStatus.StatusData.ObjectType;
    }

    // Update is called once per frame
    void Update()
    {

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

        //ノックバック
        Vector3 playerDirec = player.position - transform.position;
        playerDirec.y = 0;
        playerDirec.z = 0;

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
