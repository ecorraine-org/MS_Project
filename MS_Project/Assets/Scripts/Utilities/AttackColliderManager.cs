using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderManager : MonoBehaviour
{
    // 当たり判定を行う際に取得されるコライダーの配列
    Collider[] hitColliders;

    // 重複処理を避けるために使用される配列
    [SerializeField, Header("当たったオブジェクト配列")]
    private HashSet<Collider> hitObjects = new HashSet<Collider>();

    [SerializeField, Header("最も近いオブジェクト")]
    Collider closestCollider;

    //当たり判定可能かどうか
    bool canHit;

    /// <summary>
    /// コライダーの検出を行い、対象にダメージを与える
    /// </summary>
    public void DetectColliders(Vector3 _pos, Vector3 _size, float _damage, LayerMask _targetLayer)
    {
        if (!canHit) return;

        hitColliders = Physics.OverlapBox(_pos, _size / 2, UnityEngine.Quaternion.identity, _targetLayer);

        if (hitColliders.Length <= 0) return;

        foreach (Collider hitCollider in hitColliders)
        {
            //攻撃したオブジェクトをスキップ
            if (hitObjects.Contains(hitCollider))
            {
                continue;
            }

            //オブジェクトを格納
            hitObjects.Add(hitCollider);

            //ダメージ処理処理
            TakeDamage(hitCollider, _damage);
        }


    }

    /// <summary>
    /// オノマトペのコライダーの検出を行い、方向によって特定のオノマトペを食べる
    /// </summary>
    /// <param name="_owner">攻撃者のトランスフォーム</param>
    public void DetectCollidersWithInputDirec(Transform _owner, Vector3 _pos, Vector3 _size, float _damage, Vector3 _inputDirec, LayerMask _targetLayer)
    {
        if (!canHit) return;

        hitColliders = Physics.OverlapBox(_pos, _size / 2, UnityEngine.Quaternion.identity, _targetLayer);

        if (hitColliders.Length <= 0) return;

        //リセット
        closestCollider = null;

        //// 入力ベクトルとオノマトペの方向ベクトルの最小角度
        float minAngle = float.MaxValue;

        foreach (Collider hitCollider in hitColliders)
        {
            //攻撃したオブジェクトをスキップ
            if (hitObjects.Contains(hitCollider))
            {
                continue;
            }

            //オブジェクトを格納
            hitObjects.Add(hitCollider);

            //最も近いオブジェクトを探す
            FindClosestCollider(hitCollider, _owner, _inputDirec, ref minAngle);

        }

        // 最も近いオブジェクトに対する処理
        HandleClosestCollider(_owner, _damage);


    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    private void TakeDamage(Collider _hitCollider, float _damage)
    {
        var life = _hitCollider.GetComponentInChildren<ILife>();
        if (life != null)
        {
            life.TakeDamage(_damage);
        }

    }

    /// <summary>
    /// 最も近いオブジェクトを探す
    /// </summary>
    private void FindClosestCollider(Collider _hitCollider, Transform _owner, Vector3 _inputDirec, ref float _minAngle)
    {
        //オブジェクトへのベクトル
        Vector3 TohitCollider = _hitCollider.transform.position - _owner.position;

        //水平面の方向を取得
        TohitCollider.y = 0;
        Vector3 ToOnomotoDirec = TohitCollider.normalized;

        // 入力ベクトルとオノマトペの方向ベクトルの水平面における角度を算出する
        float angle = Vector3.Angle(ToOnomotoDirec, _inputDirec);

        // 最小角度チェック
        if (angle < _minAngle)
        {
            _minAngle = angle;
            //入力方向に最も近いオノマトペを格納
            closestCollider = _hitCollider;
        }
    }

    /// <summary>
    /// 最も近いオブジェクトに対する処理
    /// </summary>
    private void HandleClosestCollider(Transform _owner, float _damage)
    {
        if (closestCollider == null) return;

        //オノマトペへのベクトル
        Vector3 ToClosest = closestCollider.transform.position - _owner.position;

        //水平面の方向を取得
        ToClosest.y = 0;
        Vector3 ToClosestColliderDirec = ToClosest.normalized;

        //ToOnomotoDirec描画
        Debug.DrawLine(_owner.position, _owner.position + ToClosestColliderDirec, Color.blue, 1.0f);

        //食べられる処理
        closestCollider.SendMessage("Absorb");

        //ダメージ処理処理
        TakeDamage(closestCollider, _damage);
    }


    /// <summary>
    /// 当たり判定有効かを設定する
    /// </summary>
    public void SetHit(bool _canHit)
    {
        canHit = _canHit;

        //当たったオブジェクトの配列を初期化
        hitObjects.Clear();

    }

    public Collider[] HitColliders
    {
        get => this.hitColliders;
        set { this.hitColliders = value; }
    }

    public bool CanHit
    {
        get => this.canHit;
        set { this.canHit = value; }

    }
}


