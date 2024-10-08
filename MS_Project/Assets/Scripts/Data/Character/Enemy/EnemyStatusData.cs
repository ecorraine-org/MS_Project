using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Character/Enemy/EnemyStatusData", order = 2)]
public class EnemyStatusData : CharacterStatusData
{
    [Header("追跡になる距離")]
    public float fChaseDistance = 3f;

    [Header("攻撃力")]
    public float fDamage = 0;

    [Header("攻撃できる距離")]
    public float fAttackDistance = 0.8f;

    [Header("攻撃クールタイム")]
    public float fAttackCooldown = 1;

    [Header("エネミープレハブ")]
    public string enemyPrefab;

    [Header("エネミータイプ")]
    public OnomatoType eEnemyType = OnomatoType.None;

    [Header("オノマトペデータ")]
    public OnomatopoeiaData onomatoData;

    [Header("耐性")]
    public OnomatoType eTolerance = OnomatoType.None;

    [Header("SE")]
    public string sfxClip;

    [Header("VFX")]
    public string vfxClip;
}
