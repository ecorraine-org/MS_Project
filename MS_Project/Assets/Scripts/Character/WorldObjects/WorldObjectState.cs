using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObjectState : MonoBehaviour
{
    protected WorldObjectController objController;

    protected PlayerController player;

    /// <summary>
    /// ステートの初期化処理
    /// </summary>
    public virtual void Init(WorldObjectController _objectController)
    {
        if (objController != null) return;

        //マネージャー取得
        objController = _objectController;
    }

    /// <summary>
    /// ステートの更新処理
    /// </summary>
    public abstract void Tick();

    /// <summary>
    /// ステートの更新処理
    /// </summary>
    public abstract void FixedTick();

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    public abstract void Exit();
}
