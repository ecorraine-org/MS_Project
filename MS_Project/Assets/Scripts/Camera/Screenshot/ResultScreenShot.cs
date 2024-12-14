using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;

public class ResultScreenShot : MonoBehaviour
{
    [SerializeField] private Camera _screenshotCamera;
    [SerializeField] private GameObject _screenshotUI;
    [SerializeField] private float _captureDelay = 0.5f;

    private string _screenshotSavePath;
    private bool _isProcessing = false;
    private bool _isInitialized = false;

    // スクリーンショット完了時のイベント
    public event System.Action OnScreenshotCompleted;

    void Awake()
    {
        Initialize();
        FitToContainer();
    }

    private void Initialize()
    {
        if (_isInitialized) return;

        _screenshotCamera = GameObject.Find("CameraPivot(Clone)").GetComponent<Camera>();
        if (_screenshotCamera == null)
        {
            Debug.LogError("Screenshot camera not found!");
            return;
        }

        if (_screenshotUI == null)
        {
            Debug.LogError("Screenshot UI not assigned!");
            return;
        }

        string directory = Path.Combine(Application.persistentDataPath, "Screenshots");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        _isInitialized = true;
        Debug.Log("ResultScreenShot initialized successfully");
    }

    public void StartScreenshotProcess()
    {
        if (!_isInitialized)
        {
            Initialize();
        }
        StartCoroutine(CaptureAndSaveScreenshot());
    }

    private string GenerateScreenshotPath()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        string directory = Path.Combine(Application.persistentDataPath, "Screenshots");
        return Path.Combine(directory, $"screenshot_{timestamp}.png");
    }

    private IEnumerator CaptureAndSaveScreenshot()
    {
        if (_isProcessing)
        {
            Debug.LogWarning("Screenshot capture already in progress");
            yield break;
        }

        _isProcessing = true;
        Debug.Log("Starting screenshot capture process...");

        // シーンが完全に描画されるまで待機
        yield return new WaitForSeconds(_captureDelay);

        _screenshotSavePath = GenerateScreenshotPath();
        Debug.Log($"Screenshot will be saved to: {_screenshotSavePath}");

        // UI を一時的に非表示
        _screenshotUI.SetActive(false);

        // レンダリング完了まで待機
        yield return new WaitForEndOfFrame();

        bool captureSuccess = CaptureScreenshot();

        // UIを再表示
        _screenshotUI.SetActive(true);

        if (captureSuccess)
        {
            yield return new WaitForSeconds(0.1f);
            if (DisplaySavedScreenshot())
            {
                Debug.Log("Screenshot displayed successfully");
                OnScreenshotCompleted?.Invoke();
            }
            else
            {
                Debug.LogError("Failed to display screenshot");
            }
        }

        _isProcessing = false;
    }

    private bool CaptureScreenshot()
    {
        RenderTexture renderTexture = null;
        Texture2D screenShot = null;

        try
        {
            renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            RenderTexture previousRT = RenderTexture.active;

            _screenshotCamera.targetTexture = renderTexture;
            _screenshotCamera.Render();

            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenShot.Apply();

            _screenshotCamera.targetTexture = null;
            RenderTexture.active = previousRT;

            byte[] bytes = screenShot.EncodeToPNG();
            File.WriteAllBytes(_screenshotSavePath, bytes);

            Debug.Log($"Screenshot saved successfully: {_screenshotSavePath}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error during screenshot capture: {e.Message}\n{e.StackTrace}");
            return false;
        }
        finally
        {
            if (renderTexture != null)
            {
                renderTexture.Release();
                Destroy(renderTexture);
            }
            if (screenShot != null)
            {
                Destroy(screenShot);
            }
        }
    }

    private bool DisplaySavedScreenshot()
    {
        if (!File.Exists(_screenshotSavePath))
        {
            Debug.LogError($"Screenshot file not found at: {_screenshotSavePath}");
            return false;
        }

        try
        {
            byte[] fileData = File.ReadAllBytes(_screenshotSavePath);
            Texture2D texture = new Texture2D(2, 2);

            if (!texture.LoadImage(fileData))
            {
                Debug.LogError("Failed to load image data into texture");
                Destroy(texture);
                return false;
            }

            RawImage rawImage = _screenshotUI.GetComponent<RawImage>();
            if (rawImage == null)
            {
                Debug.LogError("RawImage component not found on UI object");
                Destroy(texture);
                return false;
            }

            // 既存のテクスチャをクリーンアップ
            if (rawImage.texture != null)
            {
                Destroy(rawImage.texture);
            }

            // テクスチャを設定
            rawImage.texture = texture;

            // AspectRatioFitterの取得または追加
            AspectRatioFitter aspectFitter = _screenshotUI.GetComponent<AspectRatioFitter>();
            if (aspectFitter == null)
            {
                aspectFitter = _screenshotUI.AddComponent<AspectRatioFitter>();
            }

            // AspectRatioFitterの設定
            aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            aspectFitter.aspectRatio = (float)texture.width / texture.height;

            // RectTransformの取得と設定
            RectTransform rectTransform = _screenshotUI.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // 親のRectTransformを取得
                RectTransform parentRect = rectTransform.parent as RectTransform;
                if (parentRect != null)
                {
                    // 親の大きさに合わせる
                    Vector2 parentSize = parentRect.rect.size;
                    float scale = Mathf.Min(parentSize.x / texture.width, parentSize.y / texture.height);
                    Vector2 newSize = new Vector2(texture.width * scale, texture.height * scale);
                    rectTransform.sizeDelta = newSize;
                }
            }

            Debug.Log($"Screenshot displayed with aspect ratio: {aspectFitter.aspectRatio}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error displaying screenshot: {e.Message}\n{e.StackTrace}");
            return false;
        }
    }



    private void FitToContainer()
    {
        if (_screenshotUI == null) return;

        RectTransform rectTransform = _screenshotUI.GetComponent<RectTransform>();
        if (rectTransform == null) return;

        // アンカーを中央に設定
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // AspectRatioFitterの設定
        AspectRatioFitter aspectFitter = _screenshotUI.GetComponent<AspectRatioFitter>();
        if (aspectFitter == null)
        {
            aspectFitter = _screenshotUI.AddComponent<AspectRatioFitter>();
            aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        }
    }
    private void OnDestroy()
    {
        if (_screenshotUI != null)
        {
            RawImage rawImage = _screenshotUI.GetComponent<RawImage>();
            if (rawImage != null && rawImage.texture != null)
            {
                Destroy(rawImage.texture);
            }
        }
    }


}


