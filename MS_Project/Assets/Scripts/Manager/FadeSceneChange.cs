using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSceneChange : MonoBehaviour
{
    public Image fadePanel;             // フェード用のUIパネル（Image）
    public float fadeDuration = 1.0f;   // フェードの完了にかかる時間
    private bool isFading = false;      // フェード中かどうかを判定

    private void Start()
    {
        fadePanel.enabled = true;       // フェードパネルを有効化
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0.0f); // 初期状態では透明
    }

    private void Update()
    {
        // Enterキーが押されたらフェードアウトを開始
        if (Input.GetKeyDown(KeyCode.Return) && !isFading)
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    public IEnumerator FadeOutAndLoadScene()
    {
        isFading = true;                                 // フェード中のフラグを立てる
        float elapsedTime = 0.0f;                        // 経過時間を初期化
        Color startColor = fadePanel.color;              // フェードパネルの開始色を取得
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定

        // フェードアウトアニメーションを実行
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // 経過時間を増やす
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
            yield return null;                                     // 1フレーム待機
        }

        fadePanel.color = endColor;                                // フェードが完了したら最終色に設定
        SceneManager.LoadScene("StartScene01");                    // シーンをロードしてメニューシーンに遷移
    }
}
