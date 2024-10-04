using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderManager : MonoBehaviour
{
    // オノマトペイベントのデリゲート定義
    public delegate void OnomatoEventHandler();

    // オノマトペのイベント定義
    public static event OnomatoEventHandler OnOnomatoEvent;

    Collider[] hitColliders;

    /// <summary>
    /// コライダーの検出を行い、対象にダメージを与える
    /// </summary>
    public void DetectColliders(Vector3 _pos, Vector3 _size, float _damage, LayerMask _targetLayer)
    {
        int omomatoLayer = LayerMask.NameToLayer("Onomatopoeia");

        hitColliders = Physics.OverlapBox(_pos, _size / 2, UnityEngine.Quaternion.identity, _targetLayer);

        if (hitColliders.Length <= 0) return;


        foreach (Collider hitCollider in hitColliders)
        {
            var life = hitCollider.GetComponentInChildren<ILife>();
            if (life != null)
            {
                life.TakeDamage(_damage);
            }

            //仮のオノマトペ処理
            if (hitCollider.gameObject.layer == omomatoLayer)
            {
                Debug.Log("ColliderManager:オノマトペ イベント 送信");

                //Destroy(hitCollider.gameObject);

                //オノマトペのイベント処理
                OnOnomatoEvent?.Invoke();
            }
        }
    }

    public Collider[] HitColliders
    {
        get => this.hitColliders;
        set { this.hitColliders = value; }
    }

}
