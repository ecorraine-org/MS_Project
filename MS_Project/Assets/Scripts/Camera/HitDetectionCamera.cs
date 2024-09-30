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

    //----------------
    // メンバ変数
    //----------------
    private Transform player; //プレイヤーの座標

    //画面内に存在するEnemyレイヤーの複数の敵座標
    //private List<Enemy> enemies = new List<Enemy>();
    private Camera mainCamera; //メインカメラ

    [Header("Debug ")]
    public bool showDebugVisuals = true; //デバッグ表示
    public bool enable2DDetection = true; //2D検出の有効化
    public bool enable3DDetection = true; //3D検出の有効化
    public bool enableBlendedDetection = true; //ブレンド検出の有効化
    
    //デバッグのColliderの色の設定
    public Color debugColor = Color.red;
    public Color debugColor2D = Color.green;
    public Color debugColor3D = Color.blue;
    public Color debugColorBlended = Color.yellow;



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
