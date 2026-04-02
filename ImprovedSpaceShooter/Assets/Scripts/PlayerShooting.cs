using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Muzzles")]
    [SerializeField] private Transform tip;
    [SerializeField] private Transform leftWing;
    [SerializeField] private Transform rightWing;

    [Header("Bullet Prefabs")]
    [SerializeField] private GameObject[] bulletPrefabs = new GameObject[4];

    [Header("Fire Rates")]
    [SerializeField] private float[] fireRates = new float[4];

    [Header("Spread Mode")]
    [SerializeField] private float spreadAngle = 12f;

    [Header("Burst Mode")]
    [SerializeField] private int burstCount = 4;
    [SerializeField] private float burstGap = 0.08f;

    int mode = 0;
    float nextShotTime = 0f;

    bool bursting = false;
    int burstShotsLeft = 0;
    float nextBurstShotTime = 0f;

    int baseModeBeforeTemp = 0;
    float tempModeEnd = -1f;
    bool tempActive = false;

    readonly string[] modeNames =
    {
        "Single (1)",
        "Spread (2)",
        "Burst (3)",
        "Dual Wing (4)"
    };

    void Start()
    {
        SetMode(0);
    }

    void Update()
    {
        // Hotkeys
        //if (Input.GetKeyDown(KeyCode.Alpha1)) SetMode(0);
        //if (Input.GetKeyDown(KeyCode.Alpha2)) SetMode(1);
        //if (Input.GetKeyDown(KeyCode.Alpha3)) SetMode(2);
        //if (Input.GetKeyDown(KeyCode.Alpha4)) SetMode(3);

        if (tempActive && Time.time >= tempModeEnd)
        {
            tempActive = false;
            SetMode(baseModeBeforeTemp);
        }

        if (bursting)
            UpdateBurst();

        if (Input.GetKey(KeyCode.Space))
            TryFire();
    }

    public int CurrentMode => mode;

    public void SetMode(int newMode)
    {
        mode = Mathf.Clamp(newMode, 0, 3);
        GameManagerUI.Instance?.SetMode(modeNames[mode]);
    }

    public void StartTemporaryMode(int newMode, float duration)
    {
        if (!tempActive)
            baseModeBeforeTemp = mode;

        tempActive = true;
        tempModeEnd = Time.time + duration;

        SetMode(newMode);
    }

    void TryFire()
    {
        if (bursting) return;

        float rate = fireRates[mode];
        if (rate <= 0f) return;

        float interval = 1f / rate;
        if (Time.time < nextShotTime) return;

        nextShotTime = Time.time + interval;

        AudioManager.Instance?.PlaySfx(AudioManager.Instance.shootClip);

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
        AudioManager.Instance?.PlaySfx(AudioManager.Instance.shootClip);

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
        if (!prefab) return;
        Instantiate(prefab, pos, rot);
    }
}
