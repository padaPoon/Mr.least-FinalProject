using UnityEngine;
using UnityEngine.UI;

public class Player_ui : MonoBehaviour
{
    [Header("Heart Sprites")]
    public Sprite emptyHeart;
    public Sprite fullHeart1;     // หัวใจของ Player 1
    public Sprite fullHeart2;     // หัวใจของ Player 2

    [Header("Player 1 UI")]
    public Image[] hearts;        // หัวใจของ Player 1

    [Header("Player 2 UI")]
    public Image[] hearts2;       // หัวใจของ Player 2

    private PlayerController player1;
    private PlayerController player2;

    void Start()
    {
        // หา player ทั้งหมดในฉาก
        PlayerController[] allPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        // จับคู่ตาม PlayerID
        foreach (var p in allPlayers)
        {
            if (p.playerID == PlayerController.PlayerID.Player1)
                player1 = p;
            else if (p.playerID == PlayerController.PlayerID.Player2)
                player2 = p;
        }
    }

    void Update()
    {
        if (player1 != null)
            UpdateHearts(hearts, fullHeart1, player1);

        if (player2 != null)
            UpdateHearts(hearts2, fullHeart2, player2);
    }

    void UpdateHearts(Image[] heartImages, Sprite fullSprite, PlayerController player)
    {
        if (heartImages == null) return;

        int currentHealth = player.curentHealth;

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] == null) continue;

            heartImages[i].sprite = (i < currentHealth) ? fullSprite : emptyHeart;
        }
    }
}