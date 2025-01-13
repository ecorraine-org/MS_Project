using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class GameDialog
{
    public GameObject prefab;
    public string dialogueLabel;
    [TextArea(3, 10)]
    public string dialogueText;
    public int priority;
    public Color dialogColor = Color.black; //ダイアログの色を設定するための変数
}

[System.Serializable]
public class Story
{
    [Tooltip("ストーリーの名前")]
    public string storyName;
    [Tooltip("このストーリーに含まれるプレイヤーのダイアログPrefabリスト")]
    public List<GameDialog> playerDialogPrefabs;
    [Tooltip("このストーリーに含まれるNPCのダイアログPrefabリスト")]
    public List<GameDialog> npcDialogPrefabs;
}

public class TalkManager : SingletonBaseBehavior<TalkManager>
{
    [Tooltip("ゲームUIプレハブ")]
    GameObject gameUI;

    [Tooltip("会話終了イベント定義")]
    public delegate void DialogueEventHandler();
    public static event DialogueEventHandler OnDialogueFinish;
    [Tooltip("会話処理中のフラグ")]
    private bool isProcessingDialogue = false;

    [Header("会話設定"), Tooltip("全会話のリスト")]
    public List<Story> stories; //複数のストーリーを格納

    [Header("表示設定")]
    public Transform parentTransform;
    public float playerVerticalOffset = 100f;
    public float npcVerticalOffset = 100f;
    public bool movePlayerDialogue = true;
    public bool moveNpcDialogue = true;
    public int maxDialogueCount = 5;
    [Tooltip("会話終わった後のフェイドアウト時間")]
    public float dialogueFadeOutDuration = 0.5f;
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("背景設定"), Tooltip("背景プレハブ")]
    public GameObject backgroundPrefab;

    /*
    [Header("透過設定"), Range(0f, 1f)]
    public float convoBubbleTransparency = 0.5f;
    */
    [Header("キー入力設定")]
    public float keyCooldown = 1f;      //エンターキーを押せる間隔（秒）
    private float lastPressTime = 0f;   //最後にエンターキーが押された時間

    [SerializeField, NonEditable, Header("現在のストーリー番号")]
    private int currentStoryIndex = 0;
    [SerializeField, NonEditable, Header("現在のダイアログ番号")]
    private int currentDialogIndex = 0;
    private List<GameObject> displayedPlayerInstances = new List<GameObject>();
    private List<GameObject> displayedNpcInstances = new List<GameObject>();
    private Image backgroundOverlay;
    private GameObject backgroundInstance; // 背景インスタンス

    private PlayerController player;

    protected override void AwakeProcess()
    {
    }

    void Start()
    {
        gameUI = GameObject.Find("InGameUI");

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        /*
        playerUI = gameUI.transform.Find("Canvas").gameObject;

        Transform scoreTextTransform = playerUI.transform.Find("ScoreText");

        SetUIActive(false);
        */
    }

    void Update()
    {
        //現在の時間を取得
        float currentTime = Time.unscaledTime;

        if (
            (Input.GetKeyDown(KeyCode.Return) || UIInputManager.Instance.GetEnterTrigger())
            && currentTime - lastPressTime >= keyCooldown)
        {
            //最後のエンター押下時間を更新
            lastPressTime = currentTime;
            ShowNextPrefab();
        }

        if (
            Input.GetKeyDown(KeyCode.LeftArrow)
            && currentTime - lastPressTime >= keyCooldown)
        {
            //最後のキー押下時間を更新
            lastPressTime = currentTime;
            SkipDialog();
        }

        if (player.tutorialStage == TutorialPhase.None)
        {
            if (PlayerInputManager.Instance.GetMoveDirec().magnitude > 0 && !isProcessingDialogue)
            {
                Time.timeScale = 0;
                InputController.Instance.SetInputContext(InputController.InputContext.UI);

                LoadStory(0);
                player.tutorialStage = TutorialPhase.Phase1;
            }
        }
    }

    /// <summary>
    /// ストーリー読み込み
    /// </summary>
    /// <param name="storyIndex"></param>
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

        CustomLogger.Log($"ストーリー '{stories[storyIndex].storyName}' をロードしました。");
        CreateBackgroundOverlay();
        ShowNextPrefab();
    }

    /// <summary>
    /// 次のストーリーを読み込み
    /// </summary>
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
        List<GameDialog> allDialogs = new List<GameDialog>();
        allDialogs.AddRange(currentStory.playerDialogPrefabs);
        allDialogs.AddRange(currentStory.npcDialogPrefabs);

        currentDialogIndex = allDialogs.Count;

        ShowNextPrefab();
    }

    public void ShowNextPrefab()
    {
        //処理中の場合はスキップ
        if (isProcessingDialogue) return;

        //処理を開始
        isProcessingDialogue = true;

        if (currentStoryIndex >= stories.Count)
        {
            //処理終了
            isProcessingDialogue = false;
            return;
        }

        var currentStory = stories[currentStoryIndex];
        List<GameDialog> allDialogs = new List<GameDialog>();
        allDialogs.AddRange(currentStory.playerDialogPrefabs);
        allDialogs.AddRange(currentStory.npcDialogPrefabs);

        //優先順位にソート
        allDialogs.Sort((a, b) => a.priority.CompareTo(b.priority));

        if (currentDialogIndex < allDialogs.Count)
        {
            var nextDialog = allDialogs[currentDialogIndex];
            GameObject newInstance = Instantiate(nextDialog.prefab, parentTransform ? parentTransform : transform);
            if(nextDialog.dialogueLabel.Length >=1)
                newInstance.name = nextDialog.dialogueLabel;
            if(nextDialog.dialogueText.Length >= 1)
                newInstance.GetComponentInChildren<TextDisplay>().message = nextDialog.dialogueText;

            if (currentStory.playerDialogPrefabs.Contains(nextDialog))
            {
                displayedPlayerInstances.Add(newInstance);
                if (movePlayerDialogue)
                {
                    MoveDialogInstancesUp(displayedPlayerInstances, playerVerticalOffset, nextDialog.dialogColor);
                }
                if (displayedPlayerInstances.Count > maxDialogueCount)
                {
                    StartCoroutine(FadeOutAndRemoveDialog(displayedPlayerInstances[0]));
                    displayedPlayerInstances.RemoveAt(0);
                }
            }
            else if (currentStory.npcDialogPrefabs.Contains(nextDialog))
            {
                displayedNpcInstances.Add(newInstance);
                if (moveNpcDialogue)
                {
                    MoveDialogInstancesUp(displayedNpcInstances, npcVerticalOffset, nextDialog.dialogColor);
                }
                if (displayedNpcInstances.Count > maxDialogueCount)
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
            CustomLogger.Log($"ストーリー '{stories[currentStoryIndex].storyName}' の会話が終了しました。");
            StartCoroutine(FadeOutAllDialogs());
        }

        //処理終了
        isProcessingDialogue = false;
    }

    void MoveDialogInstancesUp(List<GameObject> dialogInstances, float verticalOffset, Color dialogColor)
    {
        for (int i = 0; i < dialogInstances.Count - 1; i++)
        {
            StartCoroutine(MovePrefabsUpWithCurve(dialogInstances[i], verticalOffset, dialogColor));
        }
    }

    IEnumerator MovePrefabsUpWithCurve(GameObject prefabInstance, float offset, Color inactiveDialogColor)
    {
        //移動の時間 (秒)
        float duration = 1f;
        float elapsedTime = 0f;

        Vector3 initialPosition = prefabInstance.transform.position;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float curveValue = movementCurve.Evaluate(t);

            if (prefabInstance)
            {
                prefabInstance.transform.position = Vector3.Lerp(initialPosition, initialPosition + new Vector3(0, offset, 0), curveValue);
                ChangeDialogColor(prefabInstance, inactiveDialogColor);
                SetTransparency(prefabInstance, 1f - curveValue);
            }

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        if (prefabInstance)
        {
            prefabInstance.transform.position = initialPosition + new Vector3(0, offset, 0);
            //SetTransparency(prefabInstance, convoBubbleTransparency);
        }
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
        //なかったら終了
        if (dialogInstance == null) yield break;

        //float fadeDuration = 1f;
        float elapsedTime = 0f;

        var canvasGroup = dialogInstance.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = dialogInstance.AddComponent<CanvasGroup>();
        }

        while (elapsedTime < dialogueFadeOutDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / dialogueFadeOutDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Destroy(dialogInstance);

        /*
        //終了イベント発信
        OnDialogFinish?.Invoke();
        */
    }

    IEnumerator FadeOutAllDialogs()
    {
        /*
        float fadeDuration = 1f;
        float elapsedTime = 0f;
        */
        List<GameObject> allDialogs = new List<GameObject>(displayedPlayerInstances);
        allDialogs.AddRange(displayedNpcInstances);

        foreach (var dialog in allDialogs)
        {
            yield return StartCoroutine(FadeOutAndRemoveDialog(dialog));
        }

        //背景も非表示にする
        ShowBackgroundOverlay(false);

        //終了イベント発信
        TimerUtility.UnscaledTimeBasedTimer(this,dialogueFadeOutDuration,null,() =>
        {
            Debug.Log("会話［"+ currentStoryIndex + "］終了発信!!!");
         
           OnDialogueFinish?.Invoke();

            //仮処理
            if (currentStoryIndex == 0)
            {
                Debug.Log("最初の会話の終わり");
                InputController.Instance.SetInputContext(InputController.InputContext.Player);
                Time.timeScale = 1;
                //ui表示
                SetUIActive(true);
            }

            //チュートリアル終了
            if (currentStoryIndex == 4)
            {
                Debug.Log("美味しかったって会話8→チュートリアル終了 ");
                InputController.Instance.SetInputContext(InputController.InputContext.Player);
                Time.timeScale = 1;
                //ui表示
                SetUIActive(true);
            }
        });
    }

    void CreateBackgroundOverlay()
    {
        if (backgroundPrefab != null)
        {
            //背景プレハブをインスタンス化
            backgroundInstance = Instantiate(backgroundPrefab);
            backgroundInstance.transform.SetParent(GameObject.Find("OuterCanvas/OuterCanvasPanel").transform);
            backgroundInstance.transform.localPosition = Vector3.zero;
            backgroundInstance.transform.localScale = Vector3.one;
        }
        else
        {
            //背景プレハブが設定されていない場合は、背景色を設定
            GameObject overlayObject = new GameObject("BackgroundOverlay");
            overlayObject.transform.SetParent(GameObject.Find("OuterCanvas/OuterCanvasPanel").transform);

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
                // フェードイン
                StartCoroutine(FadeBackground(backgroundInstance, 0f, 1f));
            }
            else
            {
                StartCoroutine(FadeBackground(backgroundInstance, 1f, 0f, () =>
                {
                    //完全に消えたら非アクティブ化
                    backgroundInstance.SetActive(false);
                }));
            }
        }
        else if (backgroundOverlay != null)
        {
            if (show)
            {
                //フェードイン
                StartCoroutine(FadeBackgroundOverlay(0f, 1f));
            }
            else
            {
                StartCoroutine(FadeBackgroundOverlay(1f, 0f, () =>
                {
                    //完全に消えたら非アクティブ化
                    backgroundOverlay.gameObject.SetActive(false);
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

        //フェード時間
        float duration = dialogueFadeOutDuration;
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
        //フェード時間
        float duration = dialogueFadeOutDuration;
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

    private void SetUIActive(bool _isActive)
    {
        GameObject playerUI = gameUI.transform.Find("Canvas/Panel/PlayerUI").gameObject;
        playerUI.SetActive(_isActive);
        /*
        GameObject missionUI = gameUI.transform.Find("Canvas/Panel/MissionUI").gameObject;
        missionUI.SetActive(_isActive);
        GameObject hpSlider = gameUI.transform.Find("Canvas/Panel/HPSlider").gameObject;
        hpSlider.SetActive(_isActive);
        */
        GameObject operationDisplay = gameUI.transform.Find("Canvas/Panel/ControlsUI").gameObject;
        operationDisplay.SetActive(_isActive);

        GameObject notificationbox = gameUI.transform.Find("Canvas/Panel/NotificationUI").gameObject;
        notificationbox.SetActive(_isActive);
    }
}
