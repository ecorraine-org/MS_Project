using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Attack : EnemyAction
{
    [SerializeField, Header("突進スピード")]
    public float bushvalue = 12.0f; // 前にツッコむスピード

    public override void SkillAttack()
    {
    }

    //前にツッコむ
    public void Bush()
    {
        //Vector3 movement = transform.forward * 2.0f * Time.deltaTime;
        //rb.MovePosition(rb.position + movement);
        rb.AddForce(transform.forward * bushvalue, ForceMode.Impulse);
    }
}
