using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenshotData : MonoBehaviour
{
    public byte[] ImageData { get; private set; }
    public string FilePath { get; private set; }
    public DateTime TimeStamp { get; private set; }

    public ScreenshotData(byte[] imageData, string filePath, DateTime timeStamp)
    {
        ImageData = imageData;
        FilePath = filePath;
        TimeStamp = timeStamp;
    }
}
