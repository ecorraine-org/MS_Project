using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using PixelCrushers.SceneStreamer;

public class UIResultManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Button restartButton;

    void Start()
    {
        // GameManagerのインスタンスを通してスコアにアクセス
        if (GameManager.Instance != null)
        {
            scoreText.text = "スコア: " + GameManager.Instance.PlayerScore.ToString();
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
            scoreText.text = "スコア: 0";
        }

        restartButton.onClick.AddListener(RestartGame);
    }

    void RestartGame()
    {
        // ゲーム状態をリセット（必要に応じて）
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGameState();
        }

        // タイトルシーンに戻る
        SceneStreamerManager.TransitionScene("Title", false);
    }

    private void OnDestroy()
    {
        // ボタンのリスナーを解除
        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(RestartGame);
        }
    }
}