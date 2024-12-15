using UnityEngine;
using UnityEngine.UI;  // Text (Legacy)を使用するために必要
using System.Collections;  // IEnumeratorを使用するために必要

public class TextDisplay : MonoBehaviour
{
    [SerializeField, TextArea] private string message = "こんにちは、Unity！";  // インスペクターで複数行の入力を可能にする
    [SerializeField] private Color textColor = Color.white;  // インスペクターで色を設定可能
    [SerializeField] private int fontSize = 30;  // 文字のサイズを設定するフィールド
    public Text uiText;  // Textコンポーネントへの参照
    public float typingSpeed = 0.1f;  // 文字の表示速度

    void Start()
    {
        // Textコンポーネントの色とフォントサイズを設定
        uiText.color = textColor;
        uiText.fontSize = fontSize;

        // 自動サイズ調整を無効にする
        uiText.resizeTextForBestFit = false;
        uiText.resizeTextMinSize = fontSize;
        uiText.resizeTextMaxSize = fontSize;

        // コルーチンを呼び出して文字を一文字ずつ表示
        StartCoroutine(TypeText());
    }

    // 文字を一文字ずつ表示するコルーチン
    private IEnumerator TypeText()
    {
        uiText.text = "";  // 最初は空にする

        foreach (char letter in message)
        {
            uiText.text += letter;  // 一文字ずつ追加
            yield return new WaitForSeconds(typingSpeed);  // タイピング速度を制御
        }
    }
}