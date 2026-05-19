using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Obstacles")]
    public GameObject[] obstaclePrefab;

    [Header("Death Walls")]
    public GameObject[] deathWallPrefab;
    [Range(0f, 1f)]
    public float deathWallChance = 0.1f;

    [Header("Items")]
    public GameObject[] itemPrefabs;

    [Header("Spawn Points")]
    public Transform[] groundSpawnPoints;
    public Transform[] ceilingSpawnPoints;   

    [Header("Spawn Settings")]
    public float spawnRate = 2f;             

    [Range(0f, 1f)]
    public float itemSpawnChance = 0.3f;  

    [Range(0f, 1f)]
    public float ceilingSpawnChance = 0.5f;

    private PlayerController[] players;

    void Start()
    {
        players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        InvokeRepeating(nameof(Spawn), 0, spawnRate);
    }

    void Spawn()
    {
        if (AllPlayersGameOver()) return;

        bool spawnItem = Random.value < itemSpawnChance;

        GameObject[] prefabArray = spawnItem ? itemPrefabs : obstaclePrefab;
        if (prefabArray == null || prefabArray.Length == 0) return;
        GameObject prefab = prefabArray[Random.Range(0, prefabArray.Length)];

        bool spawnOnCeiling = Random.value < ceilingSpawnChance;

        Transform[] points = spawnOnCeiling ? ceilingSpawnPoints : groundSpawnPoints;
        if (points == null || points.Length == 0) return;

        Transform point = points[Random.Range(0, points.Length)];
        Quaternion rot = spawnOnCeiling
            ? Quaternion.Euler(0f, 0f, 180f) * prefab.transform.rotation
            : prefab.transform.rotation;
        Instantiate(prefab, point.position, rot);
        GameObject ChooseRandomPrefab()
        {
            float roll = Random.value;

            //Item
            if (roll < itemSpawnChance)
            {
                if (itemPrefabs.Length == 0) return null;
                return itemPrefabs[Random.Range(0, itemPrefabs.Length)];
            }
            //DeathWall
            else if (roll < itemSpawnChance + deathWallChance)
            {
                if (deathWallPrefab.Length == 0) return null;
                return deathWallPrefab[Random.Range(0, deathWallPrefab.Length)];
            }
            //Obstacle ปกติ
            else
            {
                if (obstaclePrefab.Length == 0) return null;
                return obstaclePrefab[Random.Range(0, obstaclePrefab.Length)];
            }
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