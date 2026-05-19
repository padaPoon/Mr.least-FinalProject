using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 10f;
    public static float speedMultiplier = 1f;

    private static PlayerController[] players;

    void Start()
    {
        speedMultiplier = 1f;
        RefreshPlayerReferences();
    }

    void Update()
    {
        // Refresh player references if they become invalid
        if (ArePlayersInvalid())
            RefreshPlayerReferences();
            
        if (AllPlayersGameOver()) return;
        transform.Translate(Vector3.left * speed * speedMultiplier * Time.deltaTime, Space.World);

        if (transform.position.x < -15 && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    void RefreshPlayerReferences()
    {
        players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
    }

    bool ArePlayersInvalid()
    {
        if (players == null || players.Length == 0) return true;
        foreach (var p in players)
        {
            if (p == null) return true; // Found a destroyed reference
        }
        return false;
    }

    bool AllPlayersGameOver()
    {
        if (players == null || players.Length == 0) return false;
        foreach (var p in players)
        {
            if (p != null && !p.gameOver) return false;
        }
        return true;
    }
}