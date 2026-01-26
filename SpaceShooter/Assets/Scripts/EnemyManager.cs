using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval;

    public float xRange;
    public float spawnZ;
    public float y = 0f;

    float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time < nextSpawnTime) return;

        nextSpawnTime = Time.time + spawnInterval;

        Vector3 pos = new Vector3(Random.Range(-xRange, xRange), y, spawnZ);
        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }
}