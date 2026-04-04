using UnityEngine;

public class FireTower : Tower
{
    [Header("Fire Stats")]
    [SerializeField] float burnDamagePerTick = 1f;
    [SerializeField] float burnDuration = 3f;
    [SerializeField] float burnTickInterval = 1f;

    [Header("Projectile")]
    [SerializeField] GameObject fireProjectilePrefab;
    [SerializeField] Transform shootPoint;

    private void Awake()
    {
        canTargetGround = true;
        canTargetFlying = true;
    }

    protected override void Attack(Enemy target)
    {
        GameObject projectileObj = Instantiate(fireProjectilePrefab, shootPoint.position, shootPoint.rotation);
        FireProjectile fireProjectile = projectileObj.GetComponent<FireProjectile>();

        fireProjectile.SetFireProjectile(target, burnDamagePerTick, burnDuration, burnTickInterval);
    }

    public override string GetTowerName()
    {
        return "Fire Tower";
    }

    public override string GetStatsText()
    {
        return "Level: " + GetTotalLevel() +
               "\nBurn Damage: " + burnDamagePerTick +
               "\nBurn Duration: " + burnDuration +
               "\nRange: " + range;
    }

    public override string GetUpgradeOptionAText()
    {
        if (upgradeALevel == 1)
        {
            return "+2 Burn Damage";
        }
        else if (upgradeALevel == 2)
        {
            return "+5 Burn Damage";
        }

        return "Maxed";
    }

    public override string GetUpgradeOptionBText()
    {
        if (upgradeBLevel == 1)
        {
            return "+ Range";
        }
        else if (upgradeBLevel == 2)
        {
            return "++ Range";
        }

        return "Maxed";
    }

    protected override void ApplyUpgradeA()
    {
        if (upgradeALevel == 1)
        {
            burnDamagePerTick += 2f;
        }
        else if (upgradeALevel == 2)
        {
            burnDamagePerTick += 5f;
        }
    }

    protected override void ApplyUpgradeB()
    {
        if (upgradeBLevel == 1)
        {
            range += 1f;
        }
        else if (upgradeBLevel == 2)
        {
            range += 1.5f;
        }
    }
}