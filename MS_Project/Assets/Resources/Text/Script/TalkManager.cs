using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogPrefab
{
    public GameObject prefab;
    public int priority;
    public Color dialogColor = Color.black; // ダイアログの色を設定するための変数
}

[System.Serializable]
public class Story
{
    public string storyName; // ストーリーの名前
    [Tooltip("このストーリーに含まれるプレイヤーのダイアログPrefabリスト")]
    public List<DialogPrefab> playerDialogPrefabs;
    [Tooltip("このストーリーに含まれるNPCのダイアログPrefabリスト")]
    public List<DialogPrefab> npcDialogPrefabs;
}

public class TalkManager : SingletonBaseBehavior<TalkManager>
{
    //会話終了イベント定義
    public delegate void DialogFinishEvtHandler();
    public static event DialogFinishEvtHandler OnDialogFinish;


    [Header("Story Settings")]
    [Tooltip("全ストーリーのリスト")]
    public List<Story> stories; // 複数のストーリーを格納

    [Header("Display Settings")]
    public Transform parentTransform;
    public float playerVerticalOffset = 100f;
    public float npcVerticalOffset = 100f;
    public bool movePlayerDialogsUp = true;
    public bool moveNpcDialogsUp = true;
    public int maxDialogCount = 5;
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Background Settings")]
    public GameObject backgroundPrefab; // 背景プレハブ

    [Header("Transparency Settings")]
    [Range(0f, 1f)]
    public float dialogTransparency = 0.7f;
    [Header("Input Cooldown Settings")]
    public float enterCooldown = 1f; // エンターキーを押せる間隔（秒）

    private float lastEnterPressTime = 0f; // 最後にエンターキーが押された時間

    [SerializeField,NonEditable,Header("現在のストーリー番号")]
    private int currentStoryIndex = 0; // 現在のストーリー番号
    [SerializeField, NonEditable, Header("現在のダイアログ番号")]
    private int currentDialogIndex = 0; // 現在のダイアログ番号
    private List<GameObject> displayedPlayerInstances = new List<GameObject>();
    private List<GameObject> displayedNpcInstances = new List<GameObject>();
    private Image backgroundOverlay;
    private GameObject backgroundInstance; // 背景インスタンス

    protected override void AwakeProcess()
    {
       // throw new System.NotImplementedException();
    }

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Time.time - lastEnterPressTime >= enterCooldown)
            {
                ShowNextPrefab();
                lastEnterPressTime = Time.time; // 押した時間を記録
            }
            else
            {
                Debug.Log("エンターキーを押すのは少し待ってください。");
            }
        }

    }

    public void LoadStory(int storyIndex)
    {
        if (storyIndex < 0 || storyIndex >= stories.Count)
        {
            Debug.LogWarning("無効なストーリーインデックスです。");
            return;
        }

        currentStoryIndex = storyIndex;
        currentDialogIndex = 0;
        displayedPlayerInstances.Clear();
        displayedNpcInstances.Clear();

        Debug.Log($"ストーリー '{stories[storyIndex].storyName}' をロードしました。");
        ShowNextPrefab();
        CreateBackgroundOverlay();

    }

    public void LoadNextStory()
    {
        int nextStoryIndex = currentStoryIndex + 1;
        if (nextStoryIndex >= stories.Count)
        {
            Debug.Log("すべてのストーリーが終了しました。");
            return;
        }

        LoadStory(nextStoryIndex);
    }

    public void SkipDialog()
    {
        var currentStory = stories[currentStoryIndex];
        List<DialogPrefab> allDialogs = new List<DialogPrefab>();
        allDialogs.AddRange(currentStory.playerDialogPrefabs);
        allDialogs.AddRange(currentStory.npcDialogPrefabs);

        currentDialogIndex = allDialogs.Count;

        ShowNextPrefab();
    }

    public void ShowNextPrefab()
    {
        if (currentStoryIndex >= stories.Count) return;

        var currentStory = stories[currentStoryIndex];
        List<DialogPrefab> allDialogs = new List<DialogPrefab>();
        allDialogs.AddRange(currentStory.playerDialogPrefabs);
        allDialogs.AddRange(currentStory.npcDialogPrefabs);

        if (currentDialogIndex < allDialogs.Count)
        {
            var nextDialog = allDialogs[currentDialogIndex];
            GameObject newInstance = Instantiate(nextDialog.prefab, parentTransform ? parentTransform : transform);

            if (currentStory.playerDialogPrefabs.Contains(nextDialog))
            {
                displayedPlayerInstances.Add(newInstance);
                if (movePlayerDialogsUp)
                {
                    MoveDialogInstancesUp(displayedPlayerInstances, playerVerticalOffset, nextDialog.dialogColor);
                }
                if (displayedPlayerInstances.Count > maxDialogCount)
                {
                    StartCoroutine(FadeOutAndRemoveDialog(displayedPlayerInstances[0]));
                    displayedPlayerInstances.RemoveAt(0);
                }
            }
            else if (currentStory.npcDialogPrefabs.Contains(nextDialog))
            {
                displayedNpcInstances.Add(newInstance);
                if (moveNpcDialogsUp)
                {
                    MoveDialogInstancesUp(displayedNpcInstances, npcVerticalOffset, nextDialog.dialogColor);
                }
                if (displayedNpcInstances.Count > maxDialogCount)
                {
                    StartCoroutine(FadeOutAndRemoveDialog(displayedNpcInstances[0]));
                    displayedNpcInstances.RemoveAt(0);
                }
            }

            ChangeDialogColor(newInstance, nextDialog.dialogColor);
            currentDialogIndex++;
        }
        else
        {
            Debug.Log($"ストーリー '{stories[currentStoryIndex].storyName}' の会話が終了しました。");
            StartCoroutine(FadeOutAllDialogs());
        }
    }

    void MoveDialogInstancesUp(List<GameObject> dialogInstances, float verticalOffset, Color dialogColor)
    {
        for (int i = 0; i < dialogInstances.Count - 1; i++)
        {
            StartCoroutine(MovePrefabsUpWithCurve(dialogInstances[i], verticalOffset, dialogColor));
        }
    }

    IEnumerator MovePrefabsUpWithCurve(GameObject prefabInstance, float offset, Color dialogColor)
    {
        float duration = 1f; // 移動の時間 (秒)　
        float elapsedTime = 0f;

        Vector3 initialPosition = prefabInstance.transform.position;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float curveValue = movementCurve.Evaluate(t);
            prefabInstance.transform.position = Vector3.Lerp(initialPosition, initialPosition + new Vector3(0, offset, 0), curveValue);
            ChangeDialogColor(prefabInstance, dialogColor);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        prefabInstance.transform.position = initialPosition + new Vector3(0, offset, 0);
        SetTransparency(prefabInstance, dialogTransparency);
    }

    void ChangeDialogColor(GameObject dialogInstance, Color color)
    {
        ChangeColorRecursively(dialogInstance.transform, color);
    }

    void ChangeColorRecursively(Transform parent, Color color)
    {
        foreach (Transform child in parent)
        {
            var textComponents = child.GetComponents<UnityEngine.UI.Text>();
            foreach (var text in textComponents)
            {
                text.color = color;
            }

            var imageComponents = child.GetComponents<UnityEngine.UI.Image>();
            foreach (var image in imageComponents)
            {
                image.color = color;
            }

            ChangeColorRecursively(child, color);
        }
    }

    void SetTransparency(GameObject dialogInstance, float transparency)
    {
        CanvasGroup canvasGroup = dialogInstance.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = dialogInstance.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = transparency;
        ChangeTransparencyRecursively(dialogInstance.transform, transparency);
    }

    void ChangeTransparencyRecursively(Transform parent, float transparency)
    {
        foreach (Transform child in parent)
        {
            var textComponents = child.GetComponents<UnityEngine.UI.Text>();
            foreach (var text in textComponents)
            {
                text.canvasRenderer.SetAlpha(transparency);
            }

            var imageComponents = child.GetComponents<UnityEngine.UI.Image>();
            foreach (var image in imageComponents)
            {
                image.canvasRenderer.SetAlpha(transparency);
            }

            ChangeTransparencyRecursively(child, transparency);
        }
    }

    IEnumerator FadeOutAndRemoveDialog(GameObject dialogInstance)
    {
        float fadeDuration = 1f;
        float elapsedTime = 0f;

        var canvasGroup = dialogInstance.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = dialogInstance.AddComponent<CanvasGroup>();
        }

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Destroy(dialogInstance);

        //終了イベント発信
        OnDialogFinish?.Invoke();
    }

    IEnumerator FadeOutAllDialogs()
    {
        float fadeDuration = 1f;
        float elapsedTime = 0f;

        List<GameObject> allDialogs = new List<GameObject>(displayedPlayerInstances);
        allDialogs.AddRange(displayedNpcInstances);

        foreach (var dialog in allDialogs)
        {
            yield return StartCoroutine(FadeOutAndRemoveDialog(dialog));
        }

        // 背景も非表示にする
        ShowBackgroundOverlay(false);

      
    }

    void CreateBackgroundOverlay()
    {
        if (backgroundPrefab != null)
        {
            // 背景プレハブをインスタンス化
            backgroundInstance = Instantiate(backgroundPrefab);
            backgroundInstance.transform.SetParent(GameObject.Find("Canvas").transform);
            backgroundInstance.transform.localPosition = Vector3.zero;
            backgroundInstance.transform.localScale = Vector3.one;
        }
        else
        {
            // 背景プレハブが設定されていない場合は、背景色を設定
            GameObject overlayObject = new GameObject("BackgroundOverlay");
            overlayObject.transform.SetParent(GameObject.Find("Canvas").transform);

            backgroundOverlay = overlayObject.AddComponent<Image>();
            backgroundOverlay.color = Color.black;
            RectTransform rectTransform = backgroundOverlay.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            backgroundOverlay.canvasRenderer.SetAlpha(1.0f);
        }
    }

    void ShowBackgroundOverlay(bool show)
    {
        if (backgroundInstance != null)
        {
            if (show)
            {
                backgroundInstance.SetActive(true);
                StartCoroutine(FadeBackground(backgroundInstance, 0f, 1f)); // フェードイン
            }
            else
            {
                StartCoroutine(FadeBackground(backgroundInstance, 1f, 0f, () =>
                {
                    backgroundInstance.SetActive(false); // 完全に消えたら非アクティブ化
                }));
            }
        }
        else if (backgroundOverlay != null)
        {
            if (show)
            {
                StartCoroutine(FadeBackgroundOverlay(0f, 1f)); // フェードイン
            }
            else
            {
                StartCoroutine(FadeBackgroundOverlay(1f, 0f, () =>
                {
                    backgroundOverlay.gameObject.SetActive(false); // 完全に消えたら非アクティブ化
                }));
            }
        }
    }

    IEnumerator FadeBackground(GameObject background, float startAlpha, float endAlpha, System.Action onComplete = null)
    {
        CanvasGroup canvasGroup = background.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = background.AddComponent<CanvasGroup>();
        }

        float duration = 1f; // フェード時間
        float elapsedTime = 0f;

        canvasGroup.alpha = startAlpha;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        onComplete?.Invoke();
    }

    IEnumerator FadeBackgroundOverlay(float startAlpha, float endAlpha, System.Action onComplete = null)
    {
        float duration = 1f; // フェード時間
        float elapsedTime = 0f;

        backgroundOverlay.canvasRenderer.SetAlpha(startAlpha);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            backgroundOverlay.canvasRenderer.SetAlpha(newAlpha);
            yield return null;
        }

        backgroundOverlay.canvasRenderer.SetAlpha(endAlpha);
        onComplete?.Invoke();
    }


}
