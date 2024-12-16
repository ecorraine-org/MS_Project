using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static InputController;

public class GameOver : MonoBehaviour
{
    public Image fadePanel;             // フェード用のUIパネル（Image）
    public float fadeDuration = 1.0f;   // フェードの完了にかかる時間
    public Image titlePanel;
    public Image continuePanel;

    public Sprite pushTitle;
    public Sprite pushStage;

    public Sprite hoverTitle;
    public Sprite hoverStage;

    bool Title;
    bool Continue;

    [SerializeField] string LoadTitle; // 切り替えるシーン名を指定
    string LoadStage; // プレイヤーがやられた時点のステージ

    private const string LastStageKey = "LastPlayedStage"; // 保存用のキー

    void Start()
    {
        fadePanel.enabled = false;       // フェードパネルを無効化
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0.0f); // 初期状態では透明

        // 保存されたステージ名を取得
        LoadStage = PlayerPrefs.GetString(LastStageKey, "DefaultStageName");
    }

    void Update()
    {
        if (UIInputManager.Instance.GetLeftTrigger())
        {
            Continue = false;
            Title = true;
        }
        if(UIInputManager.Instance.GetRightTrigger())
        {
            Title = false;
            Continue = true;
        }

        if(Title)
        {
            titlePanel.sprite = pushTitle;
        }
        if(Continue)
        {
            continuePanel.sprite = pushStage;
        }

        if ((!Title))
        {
            titlePanel.sprite = hoverTitle;
        }
        if(!Continue)
        {
            continuePanel.sprite = hoverStage;
        }

        if(Title && UIInputManager.Instance.GetEnterTrigger())
        {
            //時間経過速度
            Time.timeScale = 1;
            Destroy(GameObject.Find("CameraPivot(Clone)"));
            InputController.Instance.SetInputContext(InputContext.UI);
            StartCoroutine(FadeOutAndLoadTitle());
        }

        if(Continue && UIInputManager.Instance.GetEnterTrigger())
        {
            //時間経過速度
            Time.timeScale = 1;
            Destroy(GameObject.Find("CameraPivot(Clone)"));
            InputController.Instance.SetInputContext(InputContext.UI);
            StartCoroutine(FadeOutAndLoadStage());
        }
    }

    // ステージへのリトライ
    public IEnumerator FadeOutAndLoadStage()
    {
        fadePanel.enabled = true;   // フェードパネルを有効化s

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
        SceneManager.LoadScene(LoadStage);                    // シーンをロードしてメニューシーンに遷移
    }

    // タイトルへのフェード
     public IEnumerator FadeOutAndLoadTitle()
    {
        fadePanel.enabled = true;   // フェードパネルを有効化s

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
        SceneManager.LoadScene(LoadTitle);                    // シーンをロードしてメニューシーンに遷移
    }
}
