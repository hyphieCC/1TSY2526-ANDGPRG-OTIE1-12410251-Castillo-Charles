using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnerController : MonoBehaviour
{
    public static EnemySpawnerController Instance;

    [Header("Enemy Prefabs")]
    [SerializeField] GameObject weakGroundPrefab;
    [SerializeField] GameObject flyingPrefab;
    [SerializeField] GameObject bossPrefab;

    [Header("Spawn Setup")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform[] flyingWaypoints;

    [Header("Wave Settings")]
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] float timeBetweenWaves;
    [SerializeField] int baseEnemiesPerWave;
    [SerializeField] int bossWaveInterval;
    [SerializeField] float hpIncreasePerWave;

    int currentWave = 0;
    int activeEnemies = 0;
    bool isSpawningWave = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        currentWave++;
        GameManager.Instance.UpdateWaveUI(currentWave);
        isSpawningWave = true;

        int enemiesLeftToSpawn = baseEnemiesPerWave + (currentWave - 1) * 2;
        float hpMultiplier = 1f + (currentWave - 1) * hpIncreasePerWave;

        while (enemiesLeftToSpawn > 0)
        {
            bool spawnGroundGroup = Random.value < 0.7f;

            if (spawnGroundGroup)
            {
                int groupSize = Random.Range(2, 5);

                if (groupSize > enemiesLeftToSpawn)
                {
                    groupSize = enemiesLeftToSpawn;
                }

                SpawnGroundGroup(groupSize, hpMultiplier);
                enemiesLeftToSpawn -= groupSize;
            }
            else
            {
                SpawnSingleEnemy(flyingPrefab, hpMultiplier);
                enemiesLeftToSpawn--;
            }

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        if (currentWave % bossWaveInterval == 0)
        {
            SpawnSingleEnemy(bossPrefab, hpMultiplier);
        }

        isSpawningWave = false;
    }

    void SpawnGroundGroup(int count, float hpMultiplier)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnSingleEnemy(weakGroundPrefab, hpMultiplier);
        }
    }

    void SpawnSingleEnemy(GameObject prefab, float hpMultiplier)
    {
        GameObject enemyObj = Instantiate(
            prefab,
            RandomizePos(spawnPoint.position),
            Quaternion.identity
        );

        Enemy enemy = enemyObj.GetComponent<Enemy>();

        if (enemy is FlyingEnemy)
        {
            FlyingEnemy flyingEnemy = (FlyingEnemy)enemy;
            flyingEnemy.SetWaypoints(flyingWaypoints);
        }

        enemy.InitMonster(GameManager.Instance.GetCoreTransform().position, hpMultiplier);
        activeEnemies++;
    }

    public void EnemyRemoved()
    {
        activeEnemies--;

        if (activeEnemies <= 0 && isSpawningWave == false)
        {
            StartCoroutine(StartNextWave());
        }
    }

    Vector3 RandomizePos(Vector3 pos)
    {
        float margin = .3f;

        Vector3 newPos = new Vector3(
            Random.Range(pos.x - margin, pos.x + margin),
            pos.y,
            Random.Range(pos.z - margin, pos.z + margin)
        );

        return newPos;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}