using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLogger
{
    public static void LogWarning(object _message, object _object)
    {
        if (Debug.isDebugBuild)
            Debug.LogWarning("<color=#ffff00><b>Warning:</b> Unable to get component " + _message + "(" + _object + ")</color>");
    }

    public static void Log(object _message)
    {
        if (Debug.isDebugBuild)
            Debug.Log("<color=#00ffff>" + _message + "</color>");
    }
}
