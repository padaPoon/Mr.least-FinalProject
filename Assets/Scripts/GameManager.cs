using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private bool gameEnded = false;
    private Player_ui playerUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        playerUI = FindObjectOfType<Player_ui>();
    }

    public void PlayerLost(PlayerController.PlayerID loser)
    {
        if (gameEnded) return;
        gameEnded = true;

        // นับจำนวน player ในฉาก
        var allPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        if (allPlayers.Length <= 1)
        {
            // โหมด 1 player → Game Over ธรรมดา
            Debug.Log("💀 Game Over!");
        }
        else
        {
            // โหมด 2 player → ประกาศผู้ชนะ
            var winner = (loser == PlayerController.PlayerID.Player1)
                ? PlayerController.PlayerID.Player2
                : PlayerController.PlayerID.Player1;

            Debug.Log("🏆 " + winner + " WINS! " + loser + " crashed first.");
            
            // แสดง GameOver Panel ตามผู้ชนะ
            if (playerUI != null)
            {
                playerUI.ShowGameOverPanel(winner);
            }
        }
    }
}