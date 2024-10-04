using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Character/Enemy/EnemyStatusData", order = 1)]
public class EnemyStatusData : CharaStatusData
{
    [Header("追跡になる距離")]
    public  float chaseDistance = 3f;

    [Header("攻撃できる距離")]
    public float attackDistance = 0.8f;
}
