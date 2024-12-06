using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectController : MonoBehaviour
{
    private readonly Dictionary<string, ICameraEffect> _effects;
    private readonly List<ICameraEffect> _activeEffects;
    private readonly CameraEffectSettings _settings;

    // コンストラクタ
    public CameraEffectController(CameraEffectSettings settings)
    {
        _settings = settings;
        _effects = new Dictionary<string, ICameraEffect>();
        _activeEffects = new List<ICameraEffect>();
    }

    private void InitializeEffects()
    {
        //エフェクトを登録
        RegisterEffect("BossSubject", new BossSubjectCam());

    }

    public void RegisterEffect(string effectName, ICameraEffect effect)
    {
        if (_effects.ContainsKey(effectName))
        {
            Debug.LogWarning("Effect name is already registered: " + effectName);
        }
        _effects[effectName] = effect;
    }

    /// <summary>
    /// エフェクトを再生
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="data"></param>
    public void PlayEffect(string effectName, CameraEffectData data)
    {
        if (!_effects.TryGetValue(effectName, out var effect))
        {
            Debug.LogWarning("Effect name is not registered: " + effectName);
            return;
        }

        effect.Play(data);
        if (!_activeEffects.Contains(effect))
        {
            _activeEffects.Add(effect);
        }
    }

    public void Update()
    {
        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = _activeEffects[i];
            effect.Update();
            if (!effect.isPlaying)
            {
                _activeEffects.RemoveAt(i);
            }
        }
    }

    public T GetEffect<T>(string effectName) where T : class, ICameraEffect
    {
        if (!_effects.TryGetValue(effectName, out var effect))
        {
            Debug.LogWarning("Effect name is not registered: " + effectName);
            return null;
        }

        return effect as T;
    }

    public void StopAllEffects()
    {
        foreach (var effect in _activeEffects)
        {
            effect.Stop();
        }
        _activeEffects.Clear();
    }
}
