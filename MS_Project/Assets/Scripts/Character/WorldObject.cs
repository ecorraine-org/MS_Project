using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : MonoBehaviour, IHit, IAttack
{

    public virtual void Hit(bool _canOneHitKill) { }

    public virtual void Attack(Collider _hitCollider) { }

    /// <summary>
    /// 移動しようとする方向を取得
    /// </summary>
    public virtual Vector3 GetNextDirec() 
    {
        return transform.forward;
    }
    
}



