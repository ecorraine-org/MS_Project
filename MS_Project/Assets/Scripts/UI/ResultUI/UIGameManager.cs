using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int playerScore = 0; // スコアを保持

    public void SetScore(int score)
    {
        playerScore = score;
    }
}
