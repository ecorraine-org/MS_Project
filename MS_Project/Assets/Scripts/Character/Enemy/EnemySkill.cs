using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemySkill : MonoBehaviour
{
    public virtual void UseSkill()
    {
        if (Debug.isDebugBuild)
        {
            Debug.Log("UseSkill");
        }
    }
}
