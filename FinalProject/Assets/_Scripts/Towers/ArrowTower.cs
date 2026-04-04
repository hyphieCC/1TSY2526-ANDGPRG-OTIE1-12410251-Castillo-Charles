using UnityEngine;

public class ArrowTower : Tower
{
    [Header("Arrow Stats")]
    [SerializeField] float damage = 2f;

    [Header("Projectile")]
    [SerializeField] GameObject arrowProjectilePrefab;
    [SerializeField] Transform shootPoint;

    protected override void Attack(Enemy target)
    {
        GameObject projectileObj = Instantiate(arrowProjectilePrefab, shootPoint.position, shootPoint.rotation);
        ArrowProjectile arrowProjectile = projectileObj.GetComponent<ArrowProjectile>();
        arrowProjectile.SetProjectile(target, damage);
    }

    public override string GetTowerName()
    {
        return "Arrow Tower";
    }

    public override string GetStatsText()
    {
        return "Level: " + GetTotalLevel() +
               "\nDamage: " + damage +
               "\nRange: " + range +
               "\nAttack Rate: " + attackRate;
    }

    public override string GetUpgradeOptionAText()
    {
        if (upgradeALevel == 1)
        {
            return "+2 Damage";
        }
        else if (upgradeALevel == 2)
        {
            return "+5 Damage";
        }

        return "Maxed";
    }

    public override string GetUpgradeOptionBText()
    {
        if (upgradeBLevel == 1)
        {
            return "+ Attack Speed";
        }
        else if (upgradeBLevel == 2)
        {
            return "++ Attack Speed";
        }

        return "Maxed";
    }

    protected override void ApplyUpgradeA()
    {
        if (upgradeALevel == 1)
        {
            damage += 2f;
        }
        else if (upgradeALevel == 2)
        {
            damage += 5f;
        }
    }

    protected override void ApplyUpgradeB()
    {
        if (upgradeBLevel == 1)
        {
            attackRate -= 0.15f;
        }
        else if (upgradeBLevel == 2)
        {
            attackRate -= 0.25f;
        }

        if (attackRate < 0.2f)
        {
            attackRate = 0.2f;
        }
    }
}