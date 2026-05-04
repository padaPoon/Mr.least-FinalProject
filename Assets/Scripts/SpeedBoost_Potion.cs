using System.Collections;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float speedMultiplier = 20f;
    public float animationSpeedMultiplier = 1.5f;
    public float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.StartCoroutine(ApplySpeedBoost(player));

            Destroy(gameObject);
        }
    }

    IEnumerator ApplySpeedBoost(PlayerController player)
    {
        // set multiplier ที่ static variable ใน MoveLeft ทุก object จะเร็วขึ้นทันที รวมถึงที่ spawn ทีหลัง
        MoveLeft.speedMultiplier = speedMultiplier;

        float originalAnimSpeed = player.animator.speed;
        player.animator.speed = animationSpeedMultiplier;

        yield return new WaitForSeconds(duration);

        MoveLeft.speedMultiplier = 1f;
        player.animator.speed = originalAnimSpeed;
    }
}