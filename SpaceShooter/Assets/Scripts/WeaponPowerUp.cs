using UnityEngine;

public class WeaponPowerUp : MonoBehaviour
{
    public enum WeaponType { Single, Spread, Burst, DualWing }
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private float duration = 8f;

    [Header("Movement")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float destroyZ = -40f;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
        if (transform.position.z < destroyZ) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var shooter = other.GetComponent<PlayerShooting>();
        if (shooter != null)
        {
            shooter.StartTemporaryMode((int)weaponType, duration);
            AudioManager.Instance?.PlaySfx(AudioManager.Instance.powerUpClip);
        }

        Destroy(gameObject);
    }
}
