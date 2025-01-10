using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    [SerializeField, TextArea]
    public string message = "こんにちは、Unity！";
    [SerializeField]
    private Color fontColor = Color.white;
    [SerializeField]
    private int fontSize = 32;

    [Tooltip("Textコンポーネントへの参照")]
    public Text uiText;
    public TextMeshProUGUI uiTMP;

    [Tooltip("文字の表示速度")]
    public float typingSpeed = 0.1f;

    void Start()
    {
        //Textコンポーネントの色とフォントサイズを設定
        uiTMP.color = fontColor;
        uiTMP.fontSize = fontSize;

        /*
        uiText.color = fontColor;
        uiText.fontSize = fontSize;

        //自動サイズ調整を無効にする
        uiText.resizeTextForBestFit = false;
        uiText.resizeTextMinSize = fontSize;
        uiText.resizeTextMaxSize = fontSize;
        */

        // コルーチンを呼び出して文字を一文字ずつ表示
        StartCoroutine(TypeText());
    }

    /// <summary>
    /// 文字を一文字ずつ表示するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator TypeText()
    {
        //uiText.text = "";  // 最初は空にする
        uiTMP.text = "";

        foreach (char letter in message)
        {
            // 一文字ずつ追加
            //uiText.text += letter;
            uiTMP.text += letter;
            //yield return new WaitForSeconds(typingSpeed);  // タイピング速度を制御

            // ポーズ状態でも動作するようにカスタム待機時間を実装
            float elapsed = 0f;
            while (elapsed < typingSpeed)
            {
                // スケールされていない時間を使用して経過時間を加算
                elapsed += Time.unscaledDeltaTime;
                yield return null;  // 次のフレームまで待機
            }
        }
    }
}