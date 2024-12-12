using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全オブジェクトの基底クラス
/// </summary>
public abstract class WorldObject : MonoBehaviour, IHit, IAttack, IMiss
{
    [SerializeField, NonEditable, Header("攻撃時に使うパラメーター")]
    protected AttackerParams curAttackerParams;

    [SerializeField, NonEditable, Header("被撃時に使うパラメーター")]
    protected ReceiverParams curReceiverParams;

    [HideInInspector, Tooltip("生成するオノマトペオブジェクト")]
    protected GameObject onomatoObj;
    protected bool canGenerateOnomatopoeia = false;

    [Header("オブジェクトの親スポナー"), Tooltip("オブジェクトの親スポナー")]
    private GameObject parentSpawner;

    [Header("生成されたオノマトペ"), Tooltip("生成されたオノマトペプール")]
    public List<GameObject> onomatoPool;

    [Header("生成されたオノマトペ最大数"), Tooltip("生成されたオノマトペ最大数")]
    protected int maxOnomatopoeiaCount = 2;

    public virtual void Awake()
    {
        onomatoObj = Resources.Load<GameObject>("Onomatopoeia/OnomatoItemVariant");
        canGenerateOnomatopoeia = true;
    }

    /// <summary>
    /// 空振り処理
    /// </summary>
    public virtual void Miss() { }

    public virtual void Hit(bool _canOneHitKill) { }

    /// <summary>
    /// 攻撃受けた場合、攻撃側の攻撃データを取得
    /// </summary>
    public virtual void ReceiveHitData(AttackerParams _attackParams)
    {
        curReceiverParams.onomatoType = _attackParams.onomatoType;
        curReceiverParams.receiveDamage = _attackParams.attackDamage;
    }

    /// <summary>
    /// 攻撃した場合、自身の攻撃データ取得、(受け側に渡す)
    /// </summary>
    public virtual AttackerParams GetAttackerParams()
    {
        return curAttackerParams;
    }

    public virtual void Attack(Collider _hitCollider) { }

    /// <summary>
    /// 移動しようとする方向を取得
    /// </summary>
    public virtual Vector3 GetNextDirec()
    {
        return transform.forward;
    }

    /// <summary>
    /// 前方向を取得
    /// </summary>
    public virtual Vector3 GetForward()
    {
        return transform.forward;
    }

    public virtual Rigidbody RigidBody { get => null; }

    /// <summary>
    /// オノマトペ生成
    /// </summary>
    /// <param name="_onomatopoeiaData">オノマトペデータ</param>
    public abstract void GenerateOnomatopoeia(GameObject _owner, OnomatopoeiaData _onomatopoeiaData);



    /*
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
*/
    public GameObject ParentSpawner
    {
        get => parentSpawner;
        set { parentSpawner = value; }
    }

    public AttackerParams CurAttackerParams
    {
        get => curAttackerParams;
        set { curAttackerParams = value; }
    }

    public ReceiverParams CurReceiverParams
    {
        get => curReceiverParams;
        set { curReceiverParams = value; }
    }
}
