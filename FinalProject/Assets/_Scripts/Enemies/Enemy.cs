using System.Collections;
using UnityEngine;

public enum EnemyType
{
    Ground,
    Flying
}

public enum EnemySoundType
{
    Slime,
    Goblin,
    Raven
}

public class Enemy : MonoBehaviour
{
    protected Vector3 targetPoint;

    [SerializeField] protected EnemyType enemyType;
    [SerializeField] protected EnemySoundType enemySoundType;

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

    [Header("Burn Visual")]
    [SerializeField] GameObject burnEffectPrefab;
    GameObject activeBurnEffect;

    [Header("Slow Visual")]
    [SerializeField] GameObject slowEffectPrefab;
    GameObject activeSlowEffect;

    Renderer rend;
    Color originalColor;

    protected virtual void Start()
    {
        rend = GetComponentInChildren<Renderer>();

        if (rend != null)
        {
            originalColor = rend.material.color;
        }
    }

    protected virtual void Update()
    {
        UpdateSlow();
        UpdateBurn();
    }

    public virtual void InitMonster(Vector3 targetPoint, float hpMultiplier)
    {
        this.targetPoint = targetPoint;
        maxHp *= hpMultiplier;
        currentHp = maxHp;
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
        StartCoroutine(HitFlash());

        PlayHitSound();
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

        if (slowEffectPrefab != null && activeSlowEffect == null)
        {
            activeSlowEffect = Instantiate(slowEffectPrefab, transform);
            activeSlowEffect.transform.localPosition = new Vector3(0f, 0.25f, 0f);
        }
    }

    public void ApplyBurn(float damagePerTick, float duration, float tickInterval)
    {
        burnDamagePerTick = damagePerTick;
        burnTimer = duration;
        burnTickInterval = tickInterval;
        burnTickTimer = tickInterval;

        if (burnEffectPrefab != null && activeBurnEffect == null)
        {
            activeBurnEffect = Instantiate(burnEffectPrefab, transform);
            activeBurnEffect.transform.localPosition = new Vector3(0f, 1f, 0f);
        }
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

                if (activeSlowEffect != null)
                {
                    Destroy(activeSlowEffect);
                    activeSlowEffect = null;
                }
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

                if (activeBurnEffect != null)
                {
                    Destroy(activeBurnEffect);
                    activeBurnEffect = null;
                }
            }
        }
    }

    protected virtual void Die()
    {
        if (activeBurnEffect != null)
        {
            Destroy(activeBurnEffect);
            activeBurnEffect = null;
        }
        if (activeSlowEffect != null)
        {
            Destroy(activeSlowEffect);
            activeSlowEffect = null;
        }

        PlayDeathSound();
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

    IEnumerator HitFlash()
    {
        if (rend == null) yield break;

        float hpPercent = currentHp / maxHp;

        Color flashColor = Color.Lerp(Color.red, Color.yellow, hpPercent);
        rend.material.color = flashColor;

        yield return new WaitForSeconds(0.08f);

        rend.material.color = originalColor;
    }

    public void PlayIceFlash()
    {
        StartCoroutine(IceFlash());
    }

    IEnumerator IceFlash()
    {
        if (rend == null) yield break;

        Color flashColor = Color.cyan;

        rend.material.color = flashColor;

        float t = 0f;
        float duration = 0.1f;

        while (t < duration)
        {
            t += Time.deltaTime;
            rend.material.color = Color.Lerp(flashColor, originalColor, t / duration);
            yield return null;
        }

        rend.material.color = originalColor;
    }

    void PlayHitSound()
    {
        switch (enemySoundType)
        {
            case EnemySoundType.Slime:
                SoundManager.Instance.PlaySFX3D(
                    Random.value < 0.5f ? SoundManager.SFXType.SlimeHit1 : SoundManager.SFXType.SlimeHit2,
                    transform.position
                );
                break;

            case EnemySoundType.Goblin:
                SoundManager.Instance.PlaySFX3D(
                    Random.value < 0.5f ? SoundManager.SFXType.GoblinHit1 : SoundManager.SFXType.GoblinHit2,
                    transform.position
                );
                break;

            case EnemySoundType.Raven:
                SoundManager.Instance.PlaySFX3D(
                    Random.value < 0.5f ? SoundManager.SFXType.RavenHit1 : SoundManager.SFXType.RavenHit2,
                    transform.position
                );
                break;
        }
    }

    void PlayDeathSound()
    {
        SoundManager.Instance.PlaySFX3D(SoundManager.SFXType.GoldGained, transform.position);

        switch (enemySoundType)
        {
            case EnemySoundType.Slime:
                SoundManager.Instance.PlaySFX3D(SoundManager.SFXType.SlimeDeath, transform.position);
                break;

            case EnemySoundType.Goblin:
                SoundManager.Instance.PlaySFX3D(SoundManager.SFXType.GoblinDeath, transform.position);
                break;

            case EnemySoundType.Raven:
                SoundManager.Instance.PlaySFX3D(SoundManager.SFXType.RavenDeath, transform.position);
                break;
        }
    }
}