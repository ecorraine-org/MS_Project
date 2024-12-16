using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EatBoxSlider;

public class EatBoxSlider : MonoBehaviour
{
    [SerializeField, Header("通知BOX")]
    public RectTransform box; // 操作するPanelのRectTransform
    [SerializeField, Header("開始地点")]
    public Vector2 offScreenPosition; // Panelが画面外にあるときの座標
    [SerializeField, Header("終了地点")]
    public Vector2 onScreenPosition;  // Panelが画面内にあるときの座標
    [SerializeField, Header("スライド時間")]
    public float slideDuration; // スライドアニメーションの所要時間（秒）

    [SerializeField, Header("表示先のTextコンポーネント")]
    private TextMeshProUGUI uiText;
    [SerializeField, Header("変数に基づくテキストのマッピング")]
    private TextMapping[] textMappings; // 特定の変数に基づくテキストをマッピング
    private int currentIndex = 0; // 現在表示しているテキストのインデックス

    private bool isVisible = false; // boxが表示中かどうか
    private bool isSliding = false; // スライド中かどうか

    [System.Serializable]
    public class TextMapping
    {
        [SerializeField, Header("変数名")]
        public string variableName;
        [SerializeField, Header("対応するテキスト")]
        public string text;
    }

private void Start()
    {
        // 初期位置を設定
        if (box != null)
        {
            box.anchoredPosition = offScreenPosition;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TogglePanel();
            ShowTextByVariable();
        }
        else
        {
            Debug.LogError("UI Textコンポーネントが設定されていません。");
        }
    }
    //スライドさせる関数
    public void TogglePanel()
    {
        //if (isSliding) return; //スライド中は操作しない

        isSliding = true; //スライド中かどうか

        //isVisible = true なら offScreenPositionまでスライド、false なら onScreenPosition までスライド
        Vector2 targetPosition = isVisible ? offScreenPosition : onScreenPosition;
        StartCoroutine(SlidePanel(targetPosition));

    }

    private IEnumerator SlidePanel(Vector2 targetPosition)
    {
        float elapsedTime = 0f; //アニメーションの経過時間を追跡するための変数
        Vector2 startPosition = box.anchoredPosition; //スライド開始時のパネルの現在位置を保持

        //アニメーションが終了時間までループするよ
        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime; //前フレームから時間経過を加算
            float t = elapsedTime / slideDuration;
            box.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;

            // 割り込みチェック（必要なら条件を追加）
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("割り込み通知");
                //強制初期位置送還
                box.anchoredPosition = offScreenPosition;
                isVisible = true;
            }
        }
        box.anchoredPosition = targetPosition; //最後に目標位置にピッタリ合わせる
        isVisible = !isVisible; //真偽値の入れ替え
        isSliding = false; //スライドアニメーションの初期化
    }

    // 外部から直接テキストを設定するメソッド
    public void SetText(string newText)
    {
        if (uiText != null)
        {
            uiText.text = newText;
        }
        else
        {
            Debug.LogError("TextMeshProUGUIコンポーネントが設定されていません。");
        }
    }
    // 特定の変数に基づいてテキストを表示するメソッド
    public void ShowTextByVariable(string variable)
    {
        foreach (var mapping in textMappings)
        {
            if (mapping.variableName == variable)
            {
                SetText(mapping.text);
                return;
            }
        }
        Debug.LogWarning($"指定された変数 '{variable}' に対応するテキストが見つかりません。");
    }
}