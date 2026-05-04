using UnityEngine;

public class HealthBoost_Potion : MonoBehaviour
{
    public int healthBoostAmount = 1; // Amount of health to boost

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.curentHealth += healthBoostAmount; // Increase player's health
                Debug.Log("Health Boosted! Current Health: " + playerController.curentHealth);
                Destroy(gameObject); // Destroy the potion after use
            }
        }
    }
}
