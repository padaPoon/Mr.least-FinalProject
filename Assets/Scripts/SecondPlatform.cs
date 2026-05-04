using UnityEngine;

public class SecondPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().FlipGravity();
        }
    }
}
