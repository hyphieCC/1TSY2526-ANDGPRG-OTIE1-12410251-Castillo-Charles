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

    [Header("Stats")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected int coreDamage;
    [SerializeField] protected int goldReward;

    [Header("For Debuffs")]
    [SerializeField] protected float slowMultiplier = 1f;
    [SerializeField] protected float slowTimer = 0f;
    [SerializeField] protected float burnTimer = 0f;
    [SerializeField] protected float burnTickTimer = 0f;
    [SerializeField] protected float burnDamagePerTick = 0f;
    [SerializeField] protected float burnTickInterval = 1f;

    protected virtual void Start()
    {
        //currentHp = maxHp;
    }

    protected virtual void Update()
    {
        UpdateSlow();
        UpdateBurn();
    }

    public virtual void InitMonster(Vector3 targetPoint, float hpMultiplier)
    {
        this.targetPoint = targetPoint;
        currentHp = maxHp * hpMultiplier;
        //Debug.Log(gameObject.name + "HP: " + currentHp);
    }

    public EnemyType GetEnemyType() 
    { 
        return enemyType; 
    }

    public float GetCurrentHp()
    {
        return currentHp;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void ApplySlow(float multiplier, float duration)
    {
        slowMultiplier = multiplier;
        slowTimer = duration;
    }

    public void ApplyBurn(float damagePerTick, float duration, float tickInterval)
    {
        burnDamagePerTick = damagePerTick;
        burnTimer = duration;
        burnTickInterval = tickInterval;
        burnTickTimer = tickInterval;
    }

    public float GetSlowMultiplier()
    {
        return slowMultiplier;
    }

    void UpdateSlow()
    {
        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;

            if (slowTimer <= 0f)
            {
                slowMultiplier = 1f;
                slowTimer = 0f;
            }
        }
    }

    void UpdateBurn()
    {
        if (burnTimer > 0f)
        {
            burnTimer -= Time.deltaTime;
            burnTickTimer -= Time.deltaTime;

            if (burnTickTimer <= 0f)
            {
                TakeDamage(burnDamagePerTick);
                burnTickTimer = burnTickInterval;
            }

            if (burnTimer <= 0f)
            {
                burnTimer = 0f;
                burnDamagePerTick = 0f;
            }
        }
    }

    protected virtual void Die()
    {
        GameManager.Instance.AddGold(goldReward);
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