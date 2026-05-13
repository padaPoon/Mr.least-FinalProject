using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player_ui playerUI;

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
            // โหมด 1 player → บันทึก high score
            Debug.Log("💀 Game Over!");

            int finalScore = allPlayers[0].score;
            bool isNewRecord = ScoreManager.TrySaveHighScore(finalScore);

            if (isNewRecord)
                Debug.Log("🎉 NEW HIGH SCORE! " + finalScore);
            else
                Debug.Log($"Score: {finalScore} | High Score: {ScoreManager.GetHighScore()}");

            if (playerUI != null) playerUI.ShowGameOverPanel(loser);
            return;
        }

        // โหมด 2 player → ตัดสินจากคะแนน (ไม่บันทึก high score)
        PlayerController p1 = null, p2 = null;
        foreach (var p in allPlayers)
        {
            if (p.playerID == PlayerController.PlayerID.Player1) p1 = p;
            else if (p.playerID == PlayerController.PlayerID.Player2) p2 = p;
        }

        int p1Score = p1 != null ? p1.score : 0;
        int p2Score = p2 != null ? p2.score : 0;

        PlayerController.PlayerID winner;
        if (p1Score > p2Score)
            winner = PlayerController.PlayerID.Player1;
        else if (p2Score > p1Score)
            winner = PlayerController.PlayerID.Player2;
        else
            winner = (loser == PlayerController.PlayerID.Player1)
                ? PlayerController.PlayerID.Player2
                : PlayerController.PlayerID.Player1;

        Debug.Log($"🏆 {winner} WINS! P1: {p1Score} | P2: {p2Score}");

        if (playerUI != null) playerUI.ShowGameOverPanel(winner);
    }
}