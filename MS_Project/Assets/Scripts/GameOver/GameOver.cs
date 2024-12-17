using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static InputController;
using PixelCrushers.SceneStreamer;

public class GameOver : MonoBehaviour
{
    public Image fadePanel;             // �ｿｽt�ｿｽF�ｿｽ[�ｿｽh�ｿｽp�ｿｽ�ｿｽUI�ｿｽp�ｿｽl�ｿｽ�ｿｽ�ｿｽiImage�ｿｽj
    public float fadeDuration = 1.0f;   // �ｿｽt�ｿｽF�ｿｽ[�ｿｽh�ｿｽﾌ奇ｿｽ�ｿｽ�ｿｽ�ｿｽﾉゑｿｽ�ｿｽ�ｿｽ�ｿｽ骼橸ｿｽ�ｿｽ
    public Image titlePanel;
    public Image continuePanel;

    public Sprite pushTitle;
    public Sprite pushStage;

    public Sprite hoverTitle;
    public Sprite hoverStage;

    bool Title;
    bool Continue;

    [SerializeField] string LoadTitle; // �ｿｽﾘゑｿｽﾖゑｿｽ�ｿｽ�ｿｽV�ｿｽ[�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽw�ｿｽ�ｿｽ
    string LoadStage; // �ｿｽv�ｿｽ�ｿｽ�ｿｽC�ｿｽ�ｿｽ�ｿｽ[�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ黷ｽ�ｿｽ�ｿｽ�ｿｽ_�ｿｽﾌス�ｿｽe�ｿｽ[�ｿｽW

    private const string LastStageKey = "LastPlayedStage"; // �ｿｽﾛ托ｿｽ�ｿｽp�ｿｽﾌキ�ｿｽ[

    void Start()
    {
        fadePanel.enabled = false;       // �ｿｽt�ｿｽF�ｿｽ[�ｿｽh�ｿｽp�ｿｽl�ｿｽ�ｿｽ�ｿｽｳ鯉ｿｽ�ｿｽ�ｿｽ
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0.0f); // �ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾔでは難ｿｽ�ｿｽ�ｿｽ

        // �ｿｽﾛ托ｿｽ�ｿｽ�ｿｽ�ｿｽ黷ｽ�ｿｽX�ｿｽe�ｿｽ[�ｿｽW�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ謫ｾ
        LoadStage = PlayerPrefs.GetString(LastStageKey, "DefaultStageName");
        Debug.Log("Last played stage: " + LoadStage);
    }

    void Update()
    {
        if (UIInputManager.Instance.GetLeftTrigger())
        {
            Continue = false;
            Title = true;
        }
        if (UIInputManager.Instance.GetRightTrigger())
        {
            Title = false;
            Continue = true;
        }

        if (Title)
        {
            titlePanel.sprite = pushTitle;
        }
        if (Continue)
        {
            continuePanel.sprite = pushStage;
        }

        if ((!Title))
        {
            titlePanel.sprite = hoverTitle;
        }
        if (!Continue)
        {
            continuePanel.sprite = hoverStage;
        }

        if (Title && UIInputManager.Instance.GetEnterTrigger())
        {
            //�ｿｽ�ｿｽ�ｿｽﾔ経�ｿｽﾟ托ｿｽ�ｿｽx
            Time.timeScale = 1;
            Destroy(GameObject.Find("CameraPivot(Clone)"));
            InputController.Instance.SetInputContext(InputContext.UI);
            StartCoroutine(FadeOutAndLoadTitle());
        }

        if (Continue && UIInputManager.Instance.GetEnterTrigger())
        {
            //�ｿｽ�ｿｽ�ｿｽﾔ経�ｿｽﾟ托ｿｽ�ｿｽx
            Time.timeScale = 1;
            Destroy(GameObject.Find("CameraPivot(Clone)"));
            InputController.Instance.SetInputContext(InputContext.UI);
            StartCoroutine(FadeOutAndLoadStage());
        }
    }

    // �ｿｽX�ｿｽe�ｿｽ[�ｿｽW�ｿｽﾖの��ｿｽ�ｿｽg�ｿｽ�ｿｽ�ｿｽC
    public IEnumerator FadeOutAndLoadStage()
    {


        fadePanel.enabled = true;   // �ｿｽt�ｿｽF�ｿｽ[�ｿｽh�ｿｽp�ｿｽl�ｿｽ�ｿｽ�ｿｽ�ｿｽL�ｿｽ�ｿｽ�ｿｽ�ｿｽs

        //フェード処理
        yield return StartCoroutine(PerformFadeOut());

        // シーン遷移前のクリーンアップ
        //yield return StartCoroutine(PerformCompleteCleanup());
        SceneManager.LoadScene(LoadStage);

        // シーンの切り替え
        //SceneStreamerManager.TransitionScene(LoadStage, true);
    }

    // �ｿｽ^�ｿｽC�ｿｽg�ｿｽ�ｿｽ�ｿｽﾖのフ�ｿｽF�ｿｽ[�ｿｽh
    public IEnumerator FadeOutAndLoadTitle()
    {
        fadePanel.enabled = true;   // �ｿｽt�ｿｽF�ｿｽ[�ｿｽh�ｿｽp�ｿｽl�ｿｽ�ｿｽ�ｿｽ�ｿｽL�ｿｽ�ｿｽ�ｿｽ�ｿｽs

        float elapsedTime = 0.0f;                        // �ｿｽo�ｿｽﾟ趣ｿｽ�ｿｽﾔゑｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ
        Color startColor = fadePanel.color;              // �ｿｽt�ｿｽF�ｿｽ[�ｿｽh�ｿｽp�ｿｽl�ｿｽ�ｿｽ�ｿｽﾌ開�ｿｽn�ｿｽF�ｿｽ�ｿｽ�ｿｽ謫ｾ
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // �ｿｽt�ｿｽF�ｿｽ[�ｿｽh�ｿｽp�ｿｽl�ｿｽ�ｿｽ�ｿｽﾌ最終�ｿｽF�ｿｽ�ｿｽﾝ抵ｿｽ

        // �ｿｽt�ｿｽF�ｿｽ[�ｿｽh�ｿｽA�ｿｽE�ｿｽg�ｿｽA�ｿｽj�ｿｽ�ｿｽ�ｿｽ[�ｿｽV�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽs
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // �ｿｽo�ｿｽﾟ趣ｿｽ�ｿｽﾔを増やす
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // �ｿｽt�ｿｽF�ｿｽ[�ｿｽh�ｿｽﾌ進�ｿｽs�ｿｽx�ｿｽ�ｿｽ�ｿｽv�ｿｽZ
            fadePanel.color = Color.Lerp(startColor, endColor, t); // �ｿｽp�ｿｽl�ｿｽ�ｿｽ�ｿｽﾌ色�ｿｽ�ｿｽﾏ更�ｿｽ�ｿｽ�ｿｽﾄフ�ｿｽF�ｿｽ[�ｿｽh�ｿｽA�ｿｽE�ｿｽg
            yield return null;                                     // 1�ｿｽt�ｿｽ�ｿｽ�ｿｽ[�ｿｽ�ｿｽ�ｿｽﾒ機
        }

        fadePanel.color = endColor;                                // �ｿｽt�ｿｽF�ｿｽ[�ｿｽh�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾅ終�ｿｽF�ｿｽﾉ設抵ｿｽ
        //SceneManager.LoadScene(LoadTitle);                    // �ｿｽV�ｿｽ[�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ[�ｿｽh�ｿｽ�ｿｽ�ｿｽﾄ��ｿｽ�ｿｽj�ｿｽ�ｿｽ�ｿｽ[�ｿｽV�ｿｽ[�ｿｽ�ｿｽ�ｿｽﾉ遷�ｿｽ�ｿｽ
        SceneStreamerManager.TransitionScene(LoadTitle, true);
    }

    private void CleanupBeforeTransition()
    {
        // プレイヤー関連のクリーンアップ
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }

        // カメラのクリーンアップ
        var cameraPivot = GameObject.Find("CameraPivot(Clone)");
        if (cameraPivot != null)
        {
            Destroy(cameraPivot);
        }

        // 入力コンテキストのリセット
        InputController.Instance?.SetInputContext(InputContext.UI);

        // 時間スケールのリセット
        Time.timeScale = 1;
    }

    private IEnumerator PerformFadeOut()
    {
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
    }

    private IEnumerator PerformCompleteCleanup()
    {
        // 現在のシーンのアンロード
        SceneStreamer.UnloadAll();
        yield return new WaitForSeconds(0.1f);

        // 永続的なオブジェクトのクリーンアップ
        var persistentObjects = new string[] { "Player", "CameraPivot(Clone)", "GameManager" };
        foreach (var tag in persistentObjects)
        {
            var objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (var obj in objects)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
        }

        // シーンストリーミングのステート完全クリア
        var streamingObjects = GameObject.FindObjectsOfType<SceneStreamer>();
        foreach (var obj in streamingObjects)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }

        // メモリクリーンアップ
        yield return Resources.UnloadUnusedAssets();
        System.GC.Collect();

        // 入力とタイムスケールのリセット
        InputController.Instance?.SetInputContext(InputContext.UI);
        Time.timeScale = 1;

        yield return new WaitForEndOfFrame();
    }
}
