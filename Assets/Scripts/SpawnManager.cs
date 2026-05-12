using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoint;
    public GameObject[] obstaclePrefab;
    public float spawnRate = 2f;

    private PlayerController[] players;

    void Start()
    {
        // หา player ทุกตัวครั้งเดียว (cache)
        players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        InvokeRepeating(nameof(Spawn), 0, spawnRate);
    }

    void Spawn()
    {
        // หยุด spawn เมื่อ "ทุกคน" game over
        if (AllPlayersGameOver()) return;

        int randomIndex = Random.Range(0, obstaclePrefab.Length);
        Instantiate(
            obstaclePrefab[randomIndex],
            spawnPoint[Random.Range(0, spawnPoint.Length)].position,
            obstaclePrefab[randomIndex].transform.rotation
        );
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