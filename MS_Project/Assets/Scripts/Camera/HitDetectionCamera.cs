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
    [SerializeField] private Camera mainCamera; //メインカメラ
    [SerializeField] private LayerMask enemyLayer; //敵レイヤー
    [SerializeField] private float maxHitDistance = 5f; //最大ヒット距離
    //[SerializeField] private float visualHitThreshold = 0.1f; //視覚的なヒット閾値

    [Header("Debug ")]
    [SerializeField] private bool showDebugVisuals = true; //デバッグ表示
    [SerializeField] private bool enable2DDetection = true; //2D検出の有効化
    [SerializeField] private bool enable3DDetection = true; //3D検出の有効化
    [SerializeField] private bool enableBlendedDetection = true; //ブレンド検出の有効化

    //デバッグのColliderの色の設定
    public Color debugColor = Color.red;
    public Color debugColor2D = Color.green;
    public Color debugColor3D = Color.blue;
    public Color debugColorBlended = Color.yellow;

    //必要なコンポーネント
    private AttackColliderManager _attackColliderManager;
    private PlayerController _playerController;

    //初期化
    private void Awake()
    {
        //コンポーネントの取得(***後で取得の方法は変更する***)
        //_attackColliderManager = FindObjectOfType<AttackColliderManager>();
        //Debug.Log(_attackColliderManager);
        //_playerController = FindObjectOfType<PlayerController>();
        //Debug.Log(_playerController);
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (_attackColliderManager.HitColliders != null && _attackColliderManager.HitColliders.Length > 0)
        {

        }

        if (showDebugVisuals)
        {
            //デバッグ表示
            DrawDebugVisuals();
        }

        //補正を行う
        ApplyVisualHitCorrection();

    }

    /// <summary>
    /// 当たり判定の補正を行う関数
    /// </summary>
    private void ApplyVisualHitCorrection()
    {
        Vector3 playerPos = _playerController.transform.position;
        PlayerAttackState playerAttackState = _playerController.StateManager.CurrentState as PlayerAttackState;

        if (playerAttackState == null)
        {
            return;
        }

        //プレイヤーの攻撃範囲内だったら補正をかける
        foreach (Collider hitCollider in _attackColliderManager.HitColliders)
        {
            //3Dでの攻撃判定を表示
            if (enable3DDetection)
            {
                Vector3 hitPos = hitCollider.transform.position;
                Vector3 screenPos = mainCamera.WorldToScreenPoint(hitPos);
                Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(playerPos);

                //プレイヤーの攻撃範囲内に敵がいるかどうかを判定
                if (Vector3.Distance(screenPos, playerScreenPos) < maxHitDistance)
                {
                    //補正をかける
                    //Debug.Log("3Dでの攻撃判定を表示");
                    //Debug.Log("プレイヤーの攻撃範囲内に敵がいる");
                    //Debug.Log("補正をかける");
                }
            }
            //2Dでの攻撃判定を表示
            if (enable2DDetection)
            {
                Vector3 hitPos = hitCollider.transform.position;
                Vector3 screenPos = mainCamera.WorldToScreenPoint(hitPos);
                Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(playerPos);

                //プレイヤーの攻撃範囲内に敵がいるかどうかを判定
                if (Vector3.Distance(screenPos, playerScreenPos) < maxHitDistance)
                {
                    //補正をかける
                    //Debug.Log("2Dでの攻撃判定を表示");
                    //Debug.Log("プレイヤーの攻撃範囲内に敵がいる");
                    //Debug.Log("補正をかける");
                }
            }

            //ブレンド検出を表示
            if (enableBlendedDetection)
            {
                Vector3 hitPos = hitCollider.transform.position;
                Vector3 screenPos = mainCamera.WorldToScreenPoint(hitPos);
                Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(playerPos);

                //プレイヤーの攻撃範囲内に敵がいるかどうかを判定
                if (Vector3.Distance(screenPos, playerScreenPos) < maxHitDistance)
                {
                    //補正をかける
                    //Debug.Log("ブレンド検出を表示");
                    //Debug.Log("プレイヤーの攻撃範囲内に敵がいる");
                    //Debug.Log("補正をかける");
                }
            }


        }
    }



    private void DrawDebugVisuals()
    {
        //デバッグ表示
        if (_attackColliderManager.HitColliders != null && _attackColliderManager.HitColliders.Length > 0)
        {
            foreach (Collider hitCollider in _attackColliderManager.HitColliders)
            {
                //3Dでの攻撃判定を表示
                if (enable3DDetection)
                {
                    Vector3 hitPos = hitCollider.transform.position;
                    Vector3 screenPos = mainCamera.WorldToScreenPoint(hitPos);
                    Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(_playerController.transform.position);

                    //プレイヤーの攻撃範囲内に敵がいるかどうかを判定
                    if (Vector3.Distance(screenPos, playerScreenPos) < maxHitDistance)
                    {
                        //補正をかける
                        Debug.DrawLine(screenPos, playerScreenPos, debugColor3D);
                    }
                }
                //2Dでの攻撃判定を表示
                if (enable2DDetection)
                {
                    Vector3 hitPos = hitCollider.transform.position;
                    Vector3 screenPos = mainCamera.WorldToScreenPoint(hitPos);
                    Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(_playerController.transform.position);

                    //プレイヤーの攻撃範囲内に敵がいるかどうかを判定
                    if (Vector3.Distance(screenPos, playerScreenPos) < maxHitDistance)
                    {
                        //補正をかける
                        Debug.DrawLine(screenPos, playerScreenPos, debugColor2D);
                    }
                }

                //ブレンド検出を表示
                if (enableBlendedDetection)
                {
                    Vector3 hitPos = hitCollider.transform.position;
                    Vector3 screenPos = mainCamera.WorldToScreenPoint(hitPos);
                    Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(_playerController.transform.position);

                    //プレイヤーの攻撃範囲内に敵がいるかどうかを判定
                    if (Vector3.Distance(screenPos, playerScreenPos) < maxHitDistance)
                    {
                        //補正をかける
                        Debug.DrawLine(screenPos, playerScreenPos, debugColorBlended);
                    }
                }
            }

            //今後の実装メモ
            //HitDetectionSystem　イベントの発行を行い、それを購読することで
            //プレイヤー、敵、カメラ間での依存関係を解消する。
            //HitInfo ヒット情報をカプセル化する
            //AttackManager 攻撃の検出と結果の処理
        }
    }
}
