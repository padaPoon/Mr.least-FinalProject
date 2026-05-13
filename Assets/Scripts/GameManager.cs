using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Single_Player_ui playerUI;

    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayerLost(PlayerController.PlayerID loser)
    {
        if (gameEnded) return;
        gameEnded = true;

        var allPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        if (allPlayers.Length <= 1)
        {
            Debug.Log("Game Over! From single player mode.");

            int finalScore = allPlayers[0].score;
            bool isNewRecord = ScoreManager.TrySaveHighScore(finalScore);

            if (isNewRecord)
                Debug.Log("Score: " + finalScore);
            else
                Debug.Log($"Score: {finalScore} | High Score: {ScoreManager.GetHighScore()}");

            if (playerUI != null) playerUI.ShowGameOverPanel(loser);
            return;
        }
        PlayerController.PlayerID winner =
            (loser == PlayerController.PlayerID.Player1)
                ? PlayerController.PlayerID.Player2
                : PlayerController.PlayerID.Player1;

        Debug.Log($"{winner} WINS {loser} crashed first.");

        if (playerUI != null) 
        {
            playerUI.ShowGameOverPanel(winner);
        }
    }
}