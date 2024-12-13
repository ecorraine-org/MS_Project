using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ScreenshotManager : MonoBehaviour
{
    // シングルトン
    private static ScreenshotManager instance;
    public static ScreenshotManager Instance => instance;

    public UnityEvent<string> OnScreenshotTaken = new UnityEvent<string>();

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
