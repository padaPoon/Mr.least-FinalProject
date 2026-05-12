using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
public class Player_ui : MonoBehaviour
{
    public Sprite emptyHeart;
    public Sprite fullHeart1;
    public Sprite fullHeart2;

    public Image[] hearts;
    public Image[] hearts2;

    private PlayerController playerController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController != null)
        {
            UpdateHearts();
        }
    }

    void UpdateHearts()
    {
        int maxHealth = playerController.Maxhealth;
        int currentHealth = playerController.curentHealth;

        // Update hearts array based on current health
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart1;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }

        // Update hearts2 array if needed
        for (int i = 0; i < hearts2.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts2[i].sprite = fullHeart2;
            }
            else
            {
                hearts2[i].sprite = emptyHeart;
            }
        }
    }
}
