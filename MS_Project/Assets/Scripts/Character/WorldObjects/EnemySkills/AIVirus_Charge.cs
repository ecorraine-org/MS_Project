using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVirus_Charge : EnemyAction
{
    [SerializeField, Header("突進スピード")]
    public float bushvalue = 12.0f; // 前にツッコむスピード

    public override void Attack()
    {
        Charge();
    }

    // 前にツッコむ
    private void Charge()
    {
        enemy.RigidBody.AddForce(enemy.transform.forward * bushvalue, ForceMode.Acceleration);
    }
}
