using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UF_Fluffy : EnemyAction
{
    bool nogravity = true;

    public override void Attack()
    {
        if(nogravity)//浮いてます
        {
            Debug.Log("get out of my swamp!");
            enemy.RigidBody.useGravity = false;
            this.transform.position += transform.forward * 1.0f * Time.deltaTime;
            enemy.RigidBody.MovePosition(this.transform.position);
            //rb.AddForce(transform.up * 10f, ForceMode.Impulse);
        }
        else//浮くのはやめよう
        {
            enemy.RigidBody.useGravity = true;
        }
    }

    //浮きましょう
    public void Fluffy()
    {
        nogravity = false;
        enemy.RigidBody.useGravity = false;
        enemy.RigidBody.AddForce(transform.forward * 10.0f, ForceMode.Impulse);
        //this.transform.position += transform.forward * 10.0f * Time.deltaTime;
        //rb.MovePosition(this.transform.position);
    }
    //浮かなくていいです
    public void NotFluffy()
    {
        nogravity = true;
        enemy.RigidBody.useGravity = true;
    }
}
