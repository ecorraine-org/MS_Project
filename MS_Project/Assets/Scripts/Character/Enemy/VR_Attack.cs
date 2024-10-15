using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class VR_Attack : EnemySkill
{
    private float Front;
    private Rigidbody rb;

    Move();
}

//前にツッコむ
void Move()
{
    rb = GetComponent<Rigidbody>();

    Front = Input.GetAxis("Vertical");
    Vector3 movement = transform.forward * Front * 2.0f * Time.deltaTime;
    rb.MovePosition(rb.position + movement);
}
