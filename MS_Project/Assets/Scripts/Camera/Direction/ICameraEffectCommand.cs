using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraEffectCommand
{
    void Execute(Camera camera);
    bool IsComplete();
    void Terminate();
}
