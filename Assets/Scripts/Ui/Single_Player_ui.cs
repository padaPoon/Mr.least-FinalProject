using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class Single_Player_ui : MonoBehaviour
{
    public Sprite emptyHeart;
    public Sprite fullHeart;     // หัวใจของ Player

    public Image[] hearts;        // หัวใจของ Player
    
    public TMP_Text scoreText;  


    public TMP_Text HighestScoreText;
    public TMP_Text currentScoreText;

    public GameObject gameOverPanel;  

    PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        player.Maxhealth = hearts.Length; // กำหนด Maxhealth ตามจำนวนหัวใจใน UI
        int currentHealth = player.curentHealth;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;

            hearts[i].sprite = (i < currentHealth) ? fullHeart : emptyHeart;
        }
        
        // แสดงคะแนน
        if (scoreText != null)
        {
            scoreText.text = "Score: " + player.score;
        }
    }

    public void ShowGameOverPanel(PlayerController.PlayerID winner)
    {
        // แสดงข้อความผู้ชนะ
        if (gameOverPanel != null)
        {
            Debug.Log("Showing Game Over Panel for " + winner);
            gameOverPanel.SetActive(true);
            currentScoreText.text = "Score: " + player.score;
            
            // บันทึก high score ถ้าเป็น record ใหม่
            ScoreManager.TrySaveHighScore(player.score);
            
            // แสดง high score
            if (HighestScoreText != null)
            {
                HighestScoreText.text = "Highest : " + ScoreManager.GetHighScore();
            }
        }

        Debug.Log($"{winner} WINS!");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
       public void RestartGame()
    {
        SceneManager.LoadScene("Bank_Build_Singleplayer");
        player = null; // รีเซ็ต player เพื่อให้ Start() หาใหม่
    }
}
