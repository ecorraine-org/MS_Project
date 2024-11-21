using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dither : MonoBehaviour
{
    [SerializeField] private float _maxSmoothness = 1.0f;
    [SerializeField] private float _minSmoothness = 0.0f;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _cameraTransform;

    private void Start()
    {
        UpdateSmoothness();
    }

    private void Update()
    {
        UpdateSmoothness();
    }

    
    private void UpdateSmoothness()
    {
        if (_playerTransform == null || _cameraTransform == null)
            return;

        float distance = Vector3.Distance(_playerTransform.position, _cameraTransform.position);
       
        _maxSmoothness = 1.0f;
        _minSmoothness = 0.0f;
        float t = Mathf.InverseLerp(0.0f, 10.0f, distance); 
        float smoothness = Mathf.Lerp(_minSmoothness, _maxSmoothness, t);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material;
            mat.SetFloat("_Dithe_Transparent", smoothness);
        }
    }
}