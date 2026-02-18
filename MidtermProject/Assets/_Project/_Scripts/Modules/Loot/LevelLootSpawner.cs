using System.Collections.Generic;
using UnityEngine;

public class LevelLootSpawner : MonoBehaviour
{
    [Header("Loot Prefabs (Variants)")]
    [SerializeField] private List<LootItem> wallLootPrefabs = new();
    [SerializeField] private List<LootItem> floorLootPrefabs = new();
    [SerializeField] private List<LootItem> tableLootPrefabs = new();

    [Header("Counts (per type)")]
    [SerializeField] private int wallCount = 2;
    [SerializeField] private int floorCount = 2;
    [SerializeField] private int tableCount = 1;

    [Header("Spawn Points (optional)")]
    [SerializeField] private LootSpawnPoint[] spawnPoints;

    private void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
            spawnPoints = FindObjectsByType<LootSpawnPoint>(FindObjectsSortMode.None);

        SpawnType(LootType.Wall, wallLootPrefabs, wallCount);
        SpawnType(LootType.Floor, floorLootPrefabs, floorCount);
        SpawnType(LootType.Table, tableLootPrefabs, tableCount);
    }

    private void SpawnType(LootType type, List<LootItem> prefabs, int count)
    {
        if (prefabs == null || prefabs.Count == 0 || count <= 0) return;

        List<LootSpawnPoint> candidates = new();
        foreach (var sp in spawnPoints)
            if (sp != null && sp.Type == type) candidates.Add(sp);

        if (candidates.Count == 0) return;

        Shuffle(candidates);

        int n = Mathf.Min(count, candidates.Count);

        List<LootItem> pool = new(prefabs);
        Shuffle(pool);

        for (int i = 0; i < n; i++)
        {
            var sp = candidates[i];

            LootItem prefab = (i < pool.Count) ? pool[i] : prefabs[Random.Range(0, prefabs.Count)];
            Instantiate(prefab, sp.transform.position, sp.transform.rotation);
        }
    }

    private static void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
