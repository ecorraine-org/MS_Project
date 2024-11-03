using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectState : MonoBehaviour
{
    protected ObjectController objController;

    protected ObjectStatusHandler objStatusHandler;
    protected ObjectStateHandler objStateHandler;
    protected ObjectAnimHandler objAnimHandler;

    /// <summary>
    ///ステートの初期化処理
    /// </summary>
    public virtual void Init(ObjectController _objectController)
    {
        //マネージャー取得
        //objStatusHandler = objController.StatusManager;
        //objStateHandler = objController.StateManager;
        //objAnimHandler = objController.AnimManager;
    }

    /// <summary>
    ///ステートの更新処理
    /// </summary>
    public abstract void Tick();

    /// <summary>
    ///ステートの更新処理
    /// </summary>
    public abstract void FixedTick();

    /// <summary>
    ///ステートの終了処理
    /// </summary>
    public abstract void Exit();

}
