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

    /// <summary>
    /// オノマトペのコライダーの検出を行い、方向によって特定のオノマトペを食べる
    /// </summary>
    /// <param name="_owner">攻撃者のトランスフォーム</param>
    public void DetectOnomotoColliders(Transform _owner, Vector3 _pos, Vector3 _size, Vector3 _inputDirec, LayerMask _targetLayer)
    {

        hitColliders = Physics.OverlapBox(_pos, _size / 2, UnityEngine.Quaternion.identity, _targetLayer);

        if (hitColliders.Length <= 0) return;

        //// 入力ベクトルとオノマトペの方向ベクトルの最小角度
        float minAngle = float.MaxValue;
        //食べようとするオノマトペを格納する
        Collider closestCollider = null;

        foreach (Collider hitCollider in hitColliders)
        {

            // Debug.Log("ColliderManager:オノマトペ イベント 送信 " + hitCollider.transform.position);

            //オノマトペのイベント処理
            //  OnOnomatoEvent?.Invoke();

            //オノマトペへのベクトル
            Vector3 ToOnomoto = hitCollider.transform.position - _owner.position;

            //水平面の方向を取得
            ToOnomoto.y = 0;
            Vector3 ToOnomotoDirec = ToOnomoto.normalized;

            //ToOnomotoDirec描画
            //  Debug.DrawLine(_owner.position, _owner.position + ToOnomotoDirec, Color.blue, 1.0f);

            // 入力ベクトルとオノマトペの方向ベクトルの水平面における角度を算出する
            float angle = Vector3.Angle(ToOnomotoDirec, _inputDirec);

            // 最小角度チェック
            if (angle < minAngle)
            {
                minAngle = angle;
                //入力方向に最も近いオノマトペを格納
                closestCollider = hitCollider;
            }
        }

        //最も近いオノマトペを処理する
        if (closestCollider != null)
        {
            //オノマトペへのベクトル
            Vector3 ToOnomoto = closestCollider.transform.position - _owner.position;

            //水平面の方向を取得
            ToOnomoto.y = 0;
            Vector3 ToOnomotoDirec = ToOnomoto.normalized;

            //ToOnomotoDirec描画
            Debug.DrawLine(_owner.position, _owner.position + ToOnomotoDirec, Color.blue, 1.0f);

            //食べられる処理
            closestCollider.SendMessage("Absorb");
        }
    }

    public Collider[] HitColliders
    {
        get => this.hitColliders;
        set { this.hitColliders = value; }
    }


}
