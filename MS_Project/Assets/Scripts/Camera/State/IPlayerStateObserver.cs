using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerStateObserver
{
    /// <summary>
    /// プレイヤーステートの更新
    /// </summary>

    void OnPlayerStateUpdate(StateType playerState);
}
