using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Character/Enemy/EnemyStatusData", order = 1)]
public class EnemyStatusData : CharaStatusData
{
    [Header("�ǐՂɂȂ鋗��")]
    public  float chaseDistance = 3f;

    [Header("�U���ł��鋗��")]
    public float attackDistance = 0.8f;
}
