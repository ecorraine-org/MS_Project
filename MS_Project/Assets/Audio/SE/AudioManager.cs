using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;  // AudioSourceを追加
    public AudioClip[] soundEffects; // 再生するサウンドエフェクトのリスト

    // アニメーションイベントで呼び出す
    public void PlaySound(int index)
    {
        //
        if (index >= 0 && index < soundEffects.Length)
        {
            audioSource.PlayOneShot(soundEffects[index]);
        }
        else
        {
            Debug.LogWarning("再生されてません: " + index);
        }
    }
}
