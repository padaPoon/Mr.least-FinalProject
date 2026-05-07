using UnityEngine;

public class ImmortalPotion : MonoBehaviour
{
    public float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ActivateImmunity();
                Debug.Log("Player is now immune for " + duration + " seconds!");
            }
            Destroy(gameObject);
        }
    }
}