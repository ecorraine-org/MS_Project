using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class OnomatoDataBase : ScriptableObject
{
    [SerializeField, Header("�I�m�}�g�y�I�u�W�F�N�g�Ǘ��p���X�g")]
    public List<OnomatopoeiaData> OnomatoEnemyList = new List<OnomatopoeiaData>();
    public List<OnomatopoeiaData> OnomatoPlayerList = new List<OnomatopoeiaData>();
    public List<OnomatopoeiaData> OnomatoObjectList = new List<OnomatopoeiaData>();
}