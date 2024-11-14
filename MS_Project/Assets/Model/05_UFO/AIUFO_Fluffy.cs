using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUFO_Fluffy : EnemyAction
{
    
    public float floatSpeed = 2.0f; // UFOが目標の高さまで浮き上がる速度
    public float floatPos = 6.5f;   // プレイヤーとのY座標の間隔
    public int floatDilayTime = 160;      //  浮き始める時間
    int floatDilay = 0;

    bool noGravity = false;

    private void Update()
    {
        //距離確認
        distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);

        //攻撃射程外なら浮く
        //if (distanceToPlayer >= EnemyStatus.StatusData.attackDistance)
        //{
        //    noGravity = false;
        //}
        //else
        //{
        //    noGravity = true;
        //    //Debug.Log("get out of my swamp!");
        //}


        //浮いてるかの確認
        if(!noGravity)
        {
            //浮く関数
            Fluffy();
        }
        else
        {
            floatDilay++;
            if(floatDilayTime <= floatDilay)
            {
                Debug.Log("get out of my swamp!");
                noGravity = false;
                floatDilay = 0;
            }
        }

    }

    public void Attack()
    {
        if(noGravity)//浮いてます
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
        // プレイヤーのY座標 + offsetY を目標地点とする
        Vector3 targetPosition = new Vector3(
            enemy.transform.position.x,
            player.transform.position.y + floatPos,
            enemy.transform.position.z
        );

        // 現在位置から目標位置に向けて移動
        enemy.transform.position = Vector3.Lerp(
            enemy.transform.position,
            targetPosition,
            Time.deltaTime * floatSpeed
        );
    }
    //浮かなくていいです
    public void NotFluffy()
    {
        noGravity = true;
    }
}
