using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] public float spawnInterval = 1.2f;

    [Header("Spawn Area")]
    [SerializeField] private float xRange = 200f;
    [SerializeField] private float spawnZ = 200f;
    [SerializeField] private float y = 0f;

    [HideInInspector] public float enemySpeedOverride = -1f;

    float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time < nextSpawnTime) return;
        nextSpawnTime = Time.time + spawnInterval;

        Vector3 pos = new Vector3(Random.Range(-xRange, xRange), y, spawnZ);
        var obj = Instantiate(enemyPrefab, pos, Quaternion.identity);

        if (enemySpeedOverride > 0f && obj.TryGetComponent<Enemy>(out var enemy))
            enemy.speed = enemySpeedOverride;
    }
}
