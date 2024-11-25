using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // シーンを遷移するために必要
using TMPro; // TextMeshPro用の名前空間

public class UIResultManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // TextMeshProUGUIコンポーネント
    public Button restartButton; // リスタートボタン

    void Start()
    {
        // スコアを表示
        scoreText.text = "スコア: " + GameManager.playerScore;

        // ボタンにリスナーを追加（クリックしたときの処理）
        restartButton.onClick.AddListener(RestartGame);
    }

    void RestartGame()
    {
        // シーンをリロード（ゲームに戻る）
        SceneManager.LoadScene("Title"); // ゲームシーンの名前に変更
    }
}
