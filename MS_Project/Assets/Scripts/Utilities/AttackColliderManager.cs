using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///</note>
///初期処理:canHitを有効する必要がある
///リセット処理する必要がある
///</note>
public class AttackColliderManager : MonoBehaviour
{
    // 当たり判定を行う際に取得されるコライダーの配列
    [SerializeField, Header("当たったオブジェクト配列")]
    Collider[] hitColliders;

    // 重複処理を避けるために使用される配列
    [SerializeField, Header("当たったオブジェクト配列")]
    private HashSet<Collider> hitObjects = new HashSet<Collider>();

    [SerializeField, Header("最も近いオブジェクト")]
    Collider closestCollider;
    private CameraBasedHitCorrection _cameraBasedHitCorrection;

    [SerializeField, Header("最初の衝突が発生したかどうかを示す")]
    bool hasCollided=false;

    //当たり判定可能かどうか
    bool canHit = false;



    private void Awake()
    {
        //1. IContainerBuilderを使用して、コンポーネントを取得する

        _cameraBasedHitCorrection = GetComponent<CameraBasedHitCorrection>();
    }

    public void Reset()
    {
        canHit = false;

        // 判定終了後にリストをクリア
        hitObjects.Clear();
        hitColliders=new Collider[0];
    }

    /// <summary>
    /// コライダーの検出を行い、対象にダメージを与える
    /// </summary>
    public void DetectColliders(Vector3 _pos, Vector3 _size, float _damage, LayerMask _targetLayer, bool _oneHitKill)
    {
        //アニメーションイベントで
        //当たり判定有効するかを設定する
        if (!canHit) return;

        hitColliders = Physics.OverlapBox(_pos, _size / 2, Quaternion.identity, _targetLayer);

        if (hitColliders.Length <= 0) return;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitObjects.Contains(hitCollider)) continue;

            // bool isCorrected = _cameraBasedHitCorrection.IsHitCorrected(transform.position, hitCollider.transform.position, _size);

            // if (isCorrected)
            {
                //重複処理を避けるため
                //既に当たり判定を処理したオブジェクトをリストに入れる
                hitObjects.Add(hitCollider);
                Hit(hitCollider, _damage, _oneHitKill);
            }

            // デバッグ用の可視化
            //_cameraBasedHitCorrection.VisualizeCollider(hitCollider.transform.position, _size, isCorrected);
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
    ///被撃処理
    /// </summary>
    private void Hit(Collider _hitCollider, float _damage, bool _canOneHitKill)
    {
        var life = _hitCollider.GetComponentInChildren<ILife>();
        if (life != null)
        {
            life.TakeDamage(_damage);
        }

        //攻撃を受けた側の処理
        var hit = _hitCollider.GetComponentInChildren<IHit>();
        if (hit != null)
        {

            hit.Hit(_canOneHitKill);
        }

        //攻撃側の処理
        var attack = transform.root.GetComponentInChildren<IAttack>();
        if (attack != null)
        {
            attack.Attack(_hitCollider);
        }

        //最初の衝突が発生した
        hasCollided = true;
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
        // closestCollider.SendMessage("Absorb");


        //ダメージ処理処理
        Hit(closestCollider, _damage, false);
    }


    /// <summary>
    /// 当たり判定有効かを設定する
    /// </summary>
    public void SetHit(bool _canHit)
    {
        canHit = _canHit;

        //連続でダメージを与えるため
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

    public bool HasCollided
    {
        get => this.hasCollided;
      

    }
}


