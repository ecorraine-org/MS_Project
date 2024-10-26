using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Header("初速度")]
    protected float initialVelocity = 1;

    [SerializeField, Header("アタックコライダーマネージャー")]
    AttackColliderManager attackCollider;

    //発射初期位置
    protected Vector3 InitialPosition;

    //攻撃test
    public UnityEngine.Vector3 attackSize = new UnityEngine.Vector3(1f, 1f, 1f);
    public float attackDamage;
    public LayerMask targetLayer;

    //発射方向
    protected Vector3 shootDirec;

    private void Awake()
    {
        
    }

    public virtual void Init()
    {
        InitialPosition = transform.position;
    }

    private void Update()
    {
        //コライダーの検出
        attackCollider.DetectColliders(transform.position, attackSize, attackDamage, targetLayer, false);

        //画面内かをチェック
        CheckCamera();

        //当たると消す
        if (attackCollider.HasCollided)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void FixedUpdate()
    {
        //弾移動
        transform.position +=shootDirec * initialVelocity * Time.deltaTime;
    }

    /// <summary>
    /// 発射距離をチェック
    /// </summary>
    private void CheckDistance()
    {
        //発射距離
        float shootDistance = Vector3.Distance(transform.position, InitialPosition);

        //一定距離達したら破棄する処理
        if (shootDistance >= 10) Destroy(gameObject);
    
      
    }

    /// <summary>
    /// 画面内かをチェック
    /// </summary>
    private void CheckCamera()
    {

        Camera mainCamera = Camera.main;

        // オブジェクトの位置をカメラのビューポート座標に変換
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);

        // オブジェクトがカメラのビューポートを超えたかどうかをチェック
        if (screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1)
        {
            Destroy(gameObject); // ビューポートを超えたので、オブジェクトを破棄
        }
    }

    /// <summary>
    /// 描画test
    /// </summary>
    private void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, attackSize);   
    }
}
