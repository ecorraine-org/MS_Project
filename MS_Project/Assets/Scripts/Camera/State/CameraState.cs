using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラステートの親クラス
/// </summary>
public abstract class CameraState : MonoBehaviour
{
    // Variables
    protected Transform target; // The target object that the camera will follow
    protected Vector3 offset; // The offset position of the camera relative to the target

    //関数
    public virtual void Init(Transform _target)
    {
        target = _target;
        offset = transform.position - target.position;
    }

    //更新処理
    public abstract void Tick();

    //固定更新処理
    public abstract void FixedTick();

    //終了処理
    public abstract void Exit();
}
