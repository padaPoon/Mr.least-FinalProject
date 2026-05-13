using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 10f;
    public static float speedMultiplier = 1f;

    private static PlayerController[] players;

    void Start()
    {
        if (players == null || players.Length == 0)
            players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if (AllPlayersGameOver()) return;
        transform.Translate(Vector3.left * speed * speedMultiplier * Time.deltaTime, Space.World);

        if (transform.position.x < -15 && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
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