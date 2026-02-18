using UnityEngine;

public class LevelSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("LevelSpawnManager: spawnPoint not assigned.");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("LevelSpawnManager: No object tagged Player found.");
            return;
        }

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb == null)
            rb = player.GetComponentInParent<Rigidbody>();

        if (rb != null)
        {
            rb.position = spawnPoint.position;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            player.transform.position = spawnPoint.position;
        }

        Physics.SyncTransforms();
    }
}
