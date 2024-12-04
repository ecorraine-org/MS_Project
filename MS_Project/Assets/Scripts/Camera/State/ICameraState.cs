using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraState
{
    void EnterState(CameraStateContext context);
    void UpdateState(CameraStateContext context);
    void ExitState(CameraStateContext context);
}