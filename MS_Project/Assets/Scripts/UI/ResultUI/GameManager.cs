using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    var obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    // シーン間で保持するデータ
    private int playerScore;
    public int PlayerScore
    {
        get => playerScore;
        set
        {
            playerScore = value;
            OnScoreChanged?.Invoke(playerScore);
        }
    }

    // スコア変更時のイベント
    public event Action<int> OnScoreChanged;

    // コンポーネント参照
    private ResultTransitionHandler transitionHandler;
    public ResultTransitionHandler TransitionHandler => transitionHandler;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeComponents();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeComponents()
    {
        Debug.Log("GameManager: Initializing components");

        // ResultTransitionHandlerの初期化
        transitionHandler = FindObjectOfType<ResultTransitionHandler>();
        if (transitionHandler == null)
        {
            Debug.Log("GameManager: Creating new ResultTransitionHandler");
            var handlerObj = new GameObject("ResultTransitionHandler");
            handlerObj.transform.SetParent(transform);
            transitionHandler = handlerObj.AddComponent<ResultTransitionHandler>();
        }

        // その他の初期化処理
        ResetGameState();
    }


    public void SetScore(int score)
    {
        PlayerScore = score;
        Debug.Log($"GameManager: Score updated to {score}");
    }

    // シーン遷移前の状態保存
    public void SaveGameState()
    {
        // 必要なゲーム状態の保存処理
        PlayerPrefs.SetInt("PlayerScore", playerScore);
        PlayerPrefs.Save();
        Debug.Log("GameManager: Game state saved");
    }

    // シーン遷移後の状態復元
    public void LoadGameState()
    {
        playerScore = PlayerPrefs.GetInt("PlayerScore", 0);
        OnScoreChanged?.Invoke(playerScore);
        Debug.Log("GameManager: Game state loaded");
    }

    public void ResetGameState()
    {
        PlayerScore = 0;
        Debug.Log("GameManager: Game state reset");
    }

    // スコアの表示フォーマットを提供するヘルパーメソッド
    public string GetFormattedScore()
    {
        return $"スコア: {PlayerScore:N0}";
    }
}