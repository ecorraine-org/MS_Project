using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySkill : MonoBehaviour
{
    protected Rigidbody rb;
    public abstract void SkillAttack();

    public void SetRigidbody(Rigidbody _rb)
    {
        this.rb = _rb;
    }
}
