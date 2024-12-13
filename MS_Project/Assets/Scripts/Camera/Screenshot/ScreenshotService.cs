using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.IO;
using System;

public class ScreenshotService : IScreenshotService
{
    private const string SCREENSHOT_DIRECTORY = "Screenshots";

    public async Task CaptureScreenshotAsync(Camera camera)
    {
        string filePath = GenerateFilePath();
        await Task.Yield(); // Wait for end of frame

        RenderTexture renderTexture = null;
        try
        {
            renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture.active = renderTexture;
            Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            screenshot.Apply();

            byte[] bytes = screenshot.EncodeToPNG();
            await SaveScreenshotAsync(bytes, filePath);

            UnityEngine.Object.Destroy(screenshot); // Texture2Dのリソース解放
            ScreenshotManager.Instance.OnScreenshotTaken.Invoke(filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error capturing screenshot: {e.Message}");
            throw;
        }
        finally
        {
            if (renderTexture != null)
            {
                camera.targetTexture = null;
                RenderTexture.active = null;
                renderTexture.Release(); // RenderTextureのリソース解放
                UnityEngine.Object.Destroy(renderTexture); // Unity側のリソース解放
            }
        }
    }

    private async Task SaveScreenshotAsync(byte[] imageData, string filePath)
    {
        try
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await File.WriteAllBytesAsync(filePath, imageData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving screenshot: {e.Message}");
            throw;
        }
    }

    private string GenerateFilePath()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        return Path.Combine(Application.dataPath, SCREENSHOT_DIRECTORY, $"Screenshot_{timestamp}.png");
    }
}