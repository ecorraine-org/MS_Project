using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public interface IPlayerStateObserver
{
    void OnPlayerStateChange(PlayerState newState);
}


public class PlayerStateObserver : MonoBehaviour
{
    private PlayerState currentState;

    public void SetPlayerState(PlayerState newState)
    {
        currentState = newState;
        OnPlayerStateChange(newState);
    }

    protected virtual void OnPlayerStateChange(PlayerState newState)
    {

    }
}
