using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PixelCrushers.SceneStreamer;

public class FadeSceneChange : MonoBehaviour
{
    public Image fadePanel;             // フェード用のUIパネル（Image）
    public float fadeDuration = 1.0f;   // フェードの完了にかかる時間
    private bool isFading = false;      // フェード中かどうかを判定
    public GameObject buttonObject;     // ボタンのGameObjectを参照する変数
    private Button button;              // Buttonコンポーネントの参照

    [SerializeField] string sceneToLoad; // 切り替えるシーン名を指定

    private void Start()
    {
        fadePanel.enabled = false;       // フェードパネルを無効化
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0.0f); // 初期状態では透明
    }

    void Update()
    {
        // A,B,X,Yキーが押されたらフェードアウトを開始
        if (UIInputManager.Instance.GetAnyKeyTrigger() && !isFading)
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    // ボタン用のメソッド
    public void OnButtonClick()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    public IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;   // Enable the fade panel

        isFading = true;                                 // Set the flag indicating fading is in progress

        float elapsedTime = 0.0f;                        // Initialize the elapsed time
        Color startColor = fadePanel.color;              // Get the starting color of the fade panel
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // Set the final color of the fade panel

        // Perform the fade-out animation
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // Increase the elapsed time
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // Calculate the progress of the fade
            fadePanel.color = Color.Lerp(startColor, endColor, t); // Change the panel color to create the fade effect
            yield return null;                                     // Wait for the next frame
        }

        fadePanel.color = endColor;                                // Set the final color after the fade is complete

        SceneStreamerManager.TransitionScene(sceneToLoad, false);    // Transition to the specified scene
    }

}