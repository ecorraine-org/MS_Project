using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSceneChange : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 1.0f;
    private bool isFading = false;
    public Canvas loadingCanvas;
    [SerializeField] string sceneToLoad;

    // AnimatedSlider の参照を追加
    public AnimatedSlider animatedSlider;

    private void Start()
    {
        fadePanel.enabled = false;
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0.0f);
        loadingCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (UIInputManager.Instance.GetAnyKeyTrigger() && !isFading)
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    public void OnButtonClick()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    public IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;
        isFading = true;

        float elapsedTime = 0.0f;
        Color startColor = fadePanel.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        fadePanel.color = endColor;
        loadingCanvas.gameObject.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            // ローディング進捗に応じてスライダーを更新
            if (animatedSlider != null)
            {
                animatedSlider.UpdateSlider(asyncLoad.progress);
            }

            // 進捗が 0.9 以上になったら遷移を許可
            if (asyncLoad.progress >= 0.9f)
            {
                // 進捗がほぼ完了したら、進捗バーを100%にして遷移
                if (animatedSlider != null)
                {
                    animatedSlider.UpdateSlider(1f); // スライダーを100%に設定
                }

                // シーンをアクティブ化
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        if (loadingCanvas != null)
        {
            loadingCanvas.gameObject.SetActive(false);
        }
    }


}
