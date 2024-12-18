using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///滞空状態を判定するビヘイビア
/// </summary>
public class OnGroundSensor : MonoBehaviour
{
    [SerializeField, Header("接地判定用のコライダー")]
    BoxCollider boxCollider;
    [SerializeField, Header("当たり判定可能なレイヤー")]
    LayerMask layerMask;

    [SerializeField, NonEditable, Header("空中にいるかどうか")]
    bool isInAir = false;

    //test
    // [SerializeField, NonEditable, Header("コライダー一覧")]
    // Collider[] colliders;

    //1フレーム前の滞空状態
    bool isInAirPreFrame = false;
    //コライダー中心座標
    Vector3 center;
    //ボックスサイズの半分
    Vector3 halfExtents;


    private void Awake()
    {
        if (boxCollider == null)
        {
            //Debug.LogError("boxColliderが設定されていません", this);
            return;
        }
        ////半分のサイズで設定
        //halfExtents = boxCollider.size / 2;
    }
    private void FixedUpdate()
    {
        // 前フレームの滞空状態を保存
        isInAirPreFrame = isInAir;

        //コライダー中心座標更新
        //center = transform.position + transform.up * (halfExtents.y);
        center = boxCollider.transform.position + boxCollider.center + transform.up * (halfExtents.y);

        //半分のサイズで設定
        halfExtents = boxCollider.size / 2;

        //指定したボックスと重なっているコライダー(Groundレイヤー)をすべて見つける
        /*Collider[]*/
        // colliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, layerMask);//test

        //// 衝突しているコライダーが存在する場合、地面にいる
        //isInAir = colliders.Length > 0 ? false : true;

        // print("Length " + colliders.Length);//test
        // if (colliders.Length > 0) print("colliders " + colliders[0].name);//test
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            // print("GroundEnter " + other);//test
            // isInAirPreFrame = isInAir;
            isInAir = false;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            //print("GroundExit " + other);//test
            // isInAirPreFrame = isInAir;
            isInAir = true;

        }
    }


    //離陸瞬間
    public bool GetIsLeavingGround() { return !isInAirPreFrame && isInAir; }
    //着地瞬間
    public bool GetIsLanding() { return isInAirPreFrame && !isInAir; }

    public bool IsInAir
    {
        get => isInAir;
    }
}




//カプセルコライダー
//変数
//public CapsuleCollider capcol;
//private Vector3 point1;
//private Vector3 point2;
//private float radius;

//初期化
//radius = capcol.radius;

//更新処理
//  point1 = transform.position + transform.up * radius;
//  point2 = transform.position + transform.up * capcol.height - transform.up * radius;

//Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius, layerMask);
//if (outputCols.Length > 0)
//{
//    foreach (var col in outputCols)
//    {
//        print("collision" + col.name);
//    }
//}