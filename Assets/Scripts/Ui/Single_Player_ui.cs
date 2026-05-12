using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Single_Player_ui : MonoBehaviour
{
    public Sprite emptyHeart;
    public Sprite fullHeart;     // หัวใจของ Player

    public Image[] hearts;        // หัวใจของ Player
    
    public TMP_Text scoreText;        // ข้อความแสดงคะแนน

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
}
