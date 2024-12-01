using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitCollider : MonoBehaviour
{
    [SerializeField, Header("コライダーリスト")]
    protected List<Collider> collidersList = new List<Collider>();


    protected virtual void Update()
    {
        // ヌルチェック
        //仮処理、コストが高くなるかも
        collidersList.RemoveAll(item => item == null);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 破壊されたオブジェクトをリストから削除
       collidersList.RemoveAll(item => item == null);

        if (!collidersList.Contains(other))
        {
            collidersList.Add(other);

        }
    }


    protected virtual void OnTriggerExit(Collider other)
    {
        // 破壊されたオブジェクトをリストから削除
        collidersList.RemoveAll(item => item == null);

        if (collidersList.Contains(other))
        {
            collidersList.Remove(other);
        }

        //選ばれているときの選択解除処理
        var select = other.GetComponentInChildren<ISelected>();
        if (select != null)
        {
            select.UnSelected();
        }
    }


    public List<Collider> CollidersList
    {
        get => collidersList;
    }
}
