using UnityEngine;

public enum EnemyType
{
    Ground,
    Flying
}

public class Enemy : MonoBehaviour
{
    protected Vector3 targetPoint;

    [SerializeField] protected EnemyType enemyType;
    public EnemyType GetEnemyType() { return enemyType; }

    [Header("Stats")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected int coreDamage;
    [SerializeField] protected int goldReward;

    protected virtual void Start()
    {
        currentHp = maxHp;
    }

    public virtual void InitMonster(Vector3 targetPoint, float hpMultiplier)
    {
        this.targetPoint = targetPoint;
        currentHp = maxHp * hpMultiplier;
        //Debug.Log(gameObject.name + "HP: " + currentHp);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        EnemySpawnerController.Instance.EnemyRemoved();
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Core"))
        {
            GameManager.Instance.CoreTakeDamage(coreDamage);
            EnemySpawnerController.Instance.EnemyRemoved();
            Destroy(gameObject);
        }
    }
}