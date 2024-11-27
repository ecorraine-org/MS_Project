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

    public virtual void Awake()
    {
        onomatoObj = Resources.Load<GameObject>("Onomatopoeia/OnomatoItem");
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
}



