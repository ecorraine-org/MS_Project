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

    [SerializeField, Header("自身との距離")]
    private float distance;

    //[SerializeField, Header("自身の方向")]
   // private Vector3 direc;

    [SerializeField, Header("コライダーを検出したか")]
    private bool isColliding = false;

    [SerializeField, Header("有効か")]
    private bool isEnabled = true;

    //オブジェクトのTransform 
    //private Transform owner;

    public void DetectUpdate(Transform _owner, Vector3 _direc)
    {
        if (!isEnabled) return;

        DetectTarget(_owner, _direc);
    }

    void DetectTarget(Transform _owner, Vector3 _direc)
    {
        detectorPos = _owner.position + distance * _direc.normalized;

        //範囲内のターゲットを検出
        Collider[] colliders = Physics.OverlapCapsule(detectorPos, detectorPos, radius, targetLayer);

        if (colliders.Length > 0) isColliding = true;
        else isColliding = false;

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

    //public Vector3 Direc
    //{
    //    get => this.direc;
    //    set { this.direc = value; }
    //}

    /// <summary>
    /// 可視化処理
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(detectorPos, radius);
    }
}

