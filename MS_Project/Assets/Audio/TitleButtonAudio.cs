using UnityEngine;
using UnityEngine.UI;

public class TitleButtonAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonSound;
    private Button button;

    private void Start()
    {
        // ボタンコンポーネントの取得
        button = GetComponent<Button>();

        // AudioSourceが設定されていない場合は自動的に追加
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ボタンクリック時のイベントにSE再生処理を追加
        button.onClick.AddListener(PlayButtonSound);
    }

    private void PlayButtonSound()
    {
        if (buttonSound != null && audioSource != null)
        {
            // 再生されるオーディオクリップ名をデバッグログに表示
            Debug.Log($"再生中の音: {buttonSound.name}");

            audioSource.PlayOneShot(buttonSound);
        }
        else
        {
            Debug.LogWarning("AudioSource または AudioClip が設定されていません。");
        }
    }
}
