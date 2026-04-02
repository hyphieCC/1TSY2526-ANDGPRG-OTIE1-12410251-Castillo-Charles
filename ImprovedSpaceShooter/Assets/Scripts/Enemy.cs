using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHP = 3;
    [SerializeField] public float speed = 6f;
    [SerializeField] private float destroyZ = -40f;

    int hp;
    bool dying = false;

    void Awake() => hp = maxHP;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        if (transform.position.z < destroyZ)
            Destroy(gameObject);
    }

    public void TakeDamage(int dmg)
    {
        if (dying) return;

        hp -= dmg;
        if (hp <= 0)
        {
            dying = true;
            AudioManager.Instance?.PlaySfx(AudioManager.Instance.enemyDeathClip);
            GameManagerUI.Instance?.AddScore(1);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (dying) return;

        if (other.TryGetComponent<PlayerHealth>(out var player))
        {
            player.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
