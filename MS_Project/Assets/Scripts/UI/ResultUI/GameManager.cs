using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int playerScore = 0; // スコアを保持

    public void SetScore(int score)
    {
        playerScore = score;
    }

    private ResultTransitionHandler transitionHandler;

    private void Awake()
    {
        Debug.Log("GameManager initializing");
        // ResultTransitionHandlerが既に存在するか確認
        transitionHandler = FindObjectOfType<ResultTransitionHandler>();

        if (transitionHandler == null)
        {
            Debug.Log("Creating new ResultTransitionHandler");
            transitionHandler = gameObject.AddComponent<ResultTransitionHandler>();
        }
    }
}
