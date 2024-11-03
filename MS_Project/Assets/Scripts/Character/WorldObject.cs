using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : MonoBehaviour, IHit, IAttack
{

    public virtual void Hit(bool _canOneHitKill) { }

    public virtual void Attack(Collider _hitCollider) { }
}



