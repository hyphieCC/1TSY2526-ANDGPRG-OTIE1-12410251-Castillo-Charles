using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform tip;
    public Transform leftWing;
    public Transform rightWing;

    public GameObject[] bulletPrefabs = new GameObject[4];

    public float[] fireRates = new float[4];

    public float spreadAngle;

    public int burstCount;
    public float burstGap;

    int mode = 0;
    float nextShotTime = 0f;

    bool bursting = false;
    int burstShotsLeft = 0;
    float nextBurstShotTime = 0f;

    string[] modeNames =
    {
        "Single (1)",
        "Spread (2)",
        "Burst (3)",
        "Dual Wing (4)"
    };

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        { 
            mode = 0; 
            GameManagerUI.Instance?.SetMode(modeNames[mode]); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        { 
            mode = 1; 
            GameManagerUI.Instance?.SetMode(modeNames[mode]); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) 
        { 
            mode = 2; 
            GameManagerUI.Instance?.SetMode(modeNames[mode]); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) 
        { 
            mode = 3; 
            GameManagerUI.Instance?.SetMode(modeNames[mode]); 
        }

        if (bursting)
            UpdateBurst();

        if (Input.GetKey(KeyCode.Space))
            TryFire();
    }

    void TryFire()
    {
        if (bursting) return;

        float interval = 1f / fireRates[mode];
        if (Time.time < nextShotTime) return;
        nextShotTime = Time.time + interval;

        switch (mode)
        {
            case 0:
                Spawn(bulletPrefabs[0], tip.position, tip.rotation);
                break;
            case 1:
                FireSpreadFromTip(bulletPrefabs[1]);
                break;
            case 2:
                StartBurst();
                break;
            case 3:
                Spawn(bulletPrefabs[3], leftWing.position, leftWing.rotation);
                Spawn(bulletPrefabs[3], rightWing.position, rightWing.rotation);
                break;
        }
    }

    void FireSpreadFromTip(GameObject prefab)
    {
        Quaternion mid = tip.rotation;
        Quaternion left = tip.rotation * Quaternion.Euler(0f, -spreadAngle, 0f);
        Quaternion right = tip.rotation * Quaternion.Euler(0f, spreadAngle, 0f);

        Spawn(prefab, tip.position, left);
        Spawn(prefab, tip.position, mid);
        Spawn(prefab, tip.position, right);
    }

    void StartBurst()
    {
        bursting = true;
        burstShotsLeft = burstCount;
        nextBurstShotTime = Time.time;
    }

    void UpdateBurst()
    {
        if (Time.time < nextBurstShotTime) return;

        Spawn(bulletPrefabs[2], tip.position, tip.rotation);

        burstShotsLeft--;
        if (burstShotsLeft <= 0)
        {
            bursting = false;
            return;
        }

        nextBurstShotTime = Time.time + burstGap;
    }

    void Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        Instantiate(prefab, pos, rot);
    }
}