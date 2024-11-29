using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全オブジェクトの基底クラス
/// </summary>
public abstract class WorldObject : MonoBehaviour, IHit, IAttack,IMiss
{
    [HideInInspector, Tooltip("生成するオノマトペオブジェクト")]
    protected GameObject onomatoObj;
    private bool canGenerateOnomatopoeia = false;
    private int maxPoolCount = 2;

    [Header("生成されたオノマトペ"), Tooltip("生成されたオノマトペプール")]
    public List<GameObject> onomatoPool;

    public virtual void Awake()
    {
        onomatoObj = Resources.Load<GameObject>("Onomatopoeia/OnomatoItemVariant");
        canGenerateOnomatopoeia = true;
    }

    protected virtual void Update()
    {
        if (onomatoPool.Count < maxPoolCount)
        {
            canGenerateOnomatopoeia = true;
        }
        else
        {
            canGenerateOnomatopoeia = false;
        }
    }

    /// <summary>
    /// 空振り処理
    /// </summary>
    public virtual void Miss() { }

    public virtual void Hit(bool _canOneHitKill) { }

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
    public void GenerateOnomatopoeia(GameObject _owner, OnomatopoeiaData _onomatopoeiaData)
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
}



