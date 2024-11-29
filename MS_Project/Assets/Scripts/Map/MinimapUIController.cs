using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Imageを使うために追加
using UnityEngine.UI;

public class MinimapUIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform _minimapPanel;
    //マップの表示部分
    [SerializeField] private RawImage _minimapDisplay;
    [SerializeField] private RectTransform _playerIndicator;
    [SerializeField] private Transform _playerTransform;

    [Header("Settings")]
    [SerializeField] private Vector2 _minimapSize = new Vector2(200, 200);
    [SerializeField] private Color _playerIndicatorColor = Color.green;

    private Camera _minimapCamera;
    private RenderTexture _minimapTexture;

    private void Start()
    {
        //InitializeMinimapUI();
    }

    private void InitializeMinimapUI()
    {
        _minimapTexture = new RenderTexture(
            (int)_minimapSize.x,
            (int)_minimapSize.y,
            16,
            RenderTextureFormat.ARGB32
        );
        _minimapCamera.targetTexture = _minimapTexture;
        _minimapDisplay.texture = _minimapTexture;

        _playerIndicator.sizeDelta = new Vector2(5, 5);
        var indicatorImage = _playerIndicator.GetComponent<Image>();
        if (indicatorImage != null)
        {
            indicatorImage.color = _playerIndicatorColor;
        }
    }

    private void Update()
    {
        UpdatePlayerIndicator();
    }

    private void UpdatePlayerIndicator()
    {
        if (_minimapCamera == null || _playerIndicator == null) return;

        Vector2 screenPoint = _minimapCamera.WorldToViewportPoint(_playerTransform.position);
        _playerIndicator.anchoredPosition = new Vector2(
            (screenPoint.x - 0.5f) * _minimapSize.x,
            (screenPoint.y - 0.5f) * _minimapSize.y
        );
    }

    private void OnDestroy()
    {
        /*
        if (_minimapTexture != null)
        {
            _minimapTexture.Release();
            OnDestroy(_minimapTexture);
        }
        return;
        */
    }
}
