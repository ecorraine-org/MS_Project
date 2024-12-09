using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraEffectObserver
{
    void OnEffectStarted(string effectName);
    void OnEffectCompleted(string effectName);
}
