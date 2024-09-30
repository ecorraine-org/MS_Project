using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ヒット判定補正用のカメラクラス
/// </summary>

//まず画面内に存在するEnemyレイヤーの敵座標とプレイヤー座標、
//そしてプレイヤーの攻撃範囲を取得しプレイヤーから見た2D画面上へと変換する。
//その後、プレイヤーの攻撃範囲内に敵がいるかどうかを判定し、当たり判定の
//補正を行う。その際、補正のかけ方及び、補正される強度と範囲のデバッグ表示を行う。
public class HitDetectionCamera : MonoBehaviour
{
    //AttackState内でColliderの発生を行う。
    //Colliderの関数を用意した場合、PlayerControllerに依存してしまう。
    //シングルトンでColliderを用意し、それを参照することでプレイヤー、敵、カメラ間での
    //依存関係を解消する。
    //プレイヤーの攻撃を行うタイミングはTick=Update関数
    //それを補正するためFixedUpdate内で補正を行う。
    //用意するColliderの内訳を詳細に記述する。
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //攻撃を与える際の処理
        
    }

    void LateUpdate()
    {

    }
}
