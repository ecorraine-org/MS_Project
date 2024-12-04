using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;  // AudioSourceを追加
    public AudioClip[] soundEffects; // 再生するサウンドエフェクトのリスト

    [Header("デバッグ設定")]
    [Tooltip("AudioSource が無効かログ")]
    [SerializeField] public bool debugAudioSourceState = false; // AudioSource の状態をデバッグ

    [Tooltip("何を再生しているかログ")]
    [SerializeField] public bool debugSoundPlayback = false;    // サウンド再生ログ

    [Tooltip("無効な変数かログ")]
    [SerializeField] public bool debugInvalidIndex = false;     // 無効なインデックスの警告

    // 
    public void PlaySound(int index)
    {
        // AudioSourceが無効の場合のデバグ
        if (!audioSource.enabled)
        {
            if (debugAudioSourceState)
                Debug.LogWarning("AudioSource が無効化されている");

            audioSource.enabled = true;
        }

        // サウンドエフェクトを再生
        if (index >= 0 && index < soundEffects.Length)
        {
            // アニメーションイベントで再生
            audioSource.PlayOneShot(soundEffects[index]);

            if (debugSoundPlayback)
                Debug.Log($"再生中の音: {soundEffects[index].name} (インデックス: {index})");
        }
        else
        {
            // インデックスが無効の場合のログ
            if (debugInvalidIndex)
                Debug.LogWarning($"設定されていない変数: {index}");
        }
    }
}
