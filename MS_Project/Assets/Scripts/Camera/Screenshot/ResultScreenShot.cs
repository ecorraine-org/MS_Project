using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// リザルト画面のスクリーンショット
/// </summary>
public class ResultScreenShot : MonoBehaviour
{
    [SerializeField] private Camera _screenShotCamera;
    [SerializeField] private GameObject _screenshotUI;
    [SerializeField] private RawImage _screenshotImage;

    private IScreenshotService _screenshotService;
    private bool _isProcessing;

    private void Start()
    {
        _screenShotCamera = GameObject.Find("CameraPivot(Clone)").GetComponent<Camera>();
        _screenshotService = new ScreenshotService();
        ScreenshotManager.Instance.OnScreenshotTaken.AddListener(HandleScreenshotTaken);
    }

    private void OnDestroy()
    {
        ScreenshotManager.Instance.OnScreenshotTaken.RemoveListener(HandleScreenshotTaken);
    }

    public async Task TakeAndShowScreenshot()
    {
        if (_isProcessing) return;

        _isProcessing = true;
        try
        {
            await _screenshotService.CaptureScreenshotAsync(_screenShotCamera);
        }
        catch (Exception e)
        {
            Debug.Log($"Screenshot failed: {e.Message}");
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private void HandleScreenshotTaken(string filePath)
    {
        try
        {
            ShowScreenshot(filePath);
        }

        catch (Exception e)
        {
            Debug.Log($"Failed to show screenshot: {e.Message}");
        }
    }

    private void ShowScreenshot(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.Log($"File not found: {filePath}");
            return;
        }

        byte[] imageData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(imageData))
        {
            _screenshotImage.texture = texture;
            _screenshotUI = GameObject.Find("RImg_resultSS");
            _screenshotUI.SetActive(true);
        }
    }
}
