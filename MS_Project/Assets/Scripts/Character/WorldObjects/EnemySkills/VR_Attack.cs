using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class VR_Attack : EnemySkill
{
    private float Front;
    //private Rigidbody rb;

    //前にツッコむ
    public override void SkillAttack()
    {
        Front = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * /*Front * */2.0f * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
