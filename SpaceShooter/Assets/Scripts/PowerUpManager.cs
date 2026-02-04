using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject[] weaponPickupPrefabs; // 4 prefabs
    public float spawnInterval = 8f;

    public float xRange = 200f;
    public float spawnZ = 200f;
    public float y = 0f;

    float nextTime;

    void Update()
    {
        if (Time.time < nextTime) return;
        nextTime = Time.time + spawnInterval;

        if (weaponPickupPrefabs == null || weaponPickupPrefabs.Length == 0) return;

        var prefab = weaponPickupPrefabs[Random.Range(0, weaponPickupPrefabs.Length)];
        Vector3 pos = new Vector3(Random.Range(-xRange, xRange), y, spawnZ);
        Instantiate(prefab, pos, Quaternion.identity);
    }
}
