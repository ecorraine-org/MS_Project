using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Character/Enemy/EnemyStatusData", order = 1)]
public class EnemyStatusData : CharaStatusData
{
    [Header("’ÇÕ‚É‚È‚é‹——£")]
    public  float chaseDistance = 3f;

    [Header("UŒ‚‚Å‚«‚é‹——£")]
    public float attackDistance = 0.8f;
}
