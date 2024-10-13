using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 前方の一定距離内でコライダーを検出する
/// </summary>
public class BlockDetector : MonoBehaviour
{
    public float radius = 1.0f;  
    public float height = 2.0f;  
    public LayerMask targetLayer; 
    public Vector3 detectorPos; 
  //  public Transform detectionEndPoint;

    [SerializeField, Header("自身との距離")]
    private float distance;

    [SerializeField, Header("自身の方向")]
    private Vector3 direc;

    [SerializeField, Header("コライダーを検出したか")]
    private bool isColliding = false;

    [SerializeField, Header("有効か")]
    private bool isEnabled = true;

    //プレイヤーコントローラー
    private PlayerController player;

    public void Init(PlayerController _playerController)
    {
        player = _playerController;
    }

    private void Update()
    {
        if (!isEnabled) return;

        direc = player.CurDirecVector;

      //  if (direc == Vector3.zero) return;

        DetectEnemies();
    }

    void DetectEnemies()
    {
        detectorPos = player.transform.position + distance* direc.normalized;

        //範囲内のターゲットを検出
        Collider[] colliders = Physics.OverlapCapsule(detectorPos, detectorPos, radius, targetLayer);

        if (colliders.Length > 0) isColliding = true;
        else isColliding = false;

        //foreach (var enemy in colliders)
        //{

            //    Debug.Log("Enemy detected: " + enemy.name);//test
            //}
    }

    public bool IsEnabled
    {
        get => this.isEnabled;
        set { this.isEnabled = value; }
    }

    public bool IsColliding
    {
        get => this.isColliding;
        set { this.isColliding = value; }
    }

    public float Distance
    {
        get => this.distance;
        set { this.distance = value; }
    }

    /// <summary>
    /// 可視化処理
    /// </summary>
    private void OnDrawGizmos()
    {


       // if (detectorPos != null )
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(detectorPos, radius);

           // Gizmos.DrawLine(detectorPos + Vector3.up * radius, detectorPos + Vector3.up * radius);
           // Gizmos.DrawLine(detectorPos - Vector3.up * radius, detectorPos - Vector3.up * radius);
        }
    }
}

