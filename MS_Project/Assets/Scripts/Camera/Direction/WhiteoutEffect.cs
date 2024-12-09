using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteoutEffect : MonoBehaviour
{
    private float _intensity = 1f;
    private float _duration;
    private float _elapsedTime;
    private Material _whiteoutMaterial;

    public WhiteoutEffect(float duration)
    {
        _duration = duration;
        _whiteoutMaterial = new Material(Shader.Find("Hidden/WhiteoutEffect"));
    }

    public void Execute(Camera camera)
    {
        _elapsedTime += Time.deltaTime;
        float normalizedTime = _elapsedTime / _duration;
        float currentIntensity = Mathf.Lerp(1f, 0f, normalizedTime);

        _whiteoutMaterial.SetFloat("_Intensity", currentIntensity);
        // camera.GetComponent<PostProcessVolume>()?.profile.AddSettings(_whiteoutMaterial);
    }

    public bool IsComplete()
    {
        return _elapsedTime >= _duration;
    }

    public void Terminate()
    {
        if (_whiteoutMaterial != null)
        {
            Object.Destroy(_whiteoutMaterial);
        }
    }
}
