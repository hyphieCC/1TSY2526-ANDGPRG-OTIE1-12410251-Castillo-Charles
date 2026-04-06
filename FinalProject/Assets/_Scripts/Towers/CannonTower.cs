using UnityEngine;

public class CannonTower : Tower
{
    [Header("Cannon Stats")]
    [SerializeField] float damage = 6f;
    [SerializeField] float splashRadius = 2.5f;
    [SerializeField] float splashDamage = 3f;

    [Header("Projectile")]
    [SerializeField] GameObject cannonProjectilePrefab;
    [SerializeField] Transform shootPoint;

    private void Awake()
    {
        canTargetGround = true;
        canTargetFlying = false;
    }

    protected override void Attack(Enemy target)
    {
        SoundManager.Instance.PlaySFX3D(SoundManager.SFXType.CannonShoot, shootPoint.position);

        GameObject projectileObj = Instantiate(cannonProjectilePrefab, shootPoint.position, shootPoint.rotation);
        CannonProjectile cannonProjectile = projectileObj.GetComponent<CannonProjectile>();
        cannonProjectile.SetCannonProjectile(target, damage, splashRadius, splashDamage);
    }

    public override string GetTowerName()
    {
        return "Cannon Tower";
    }

    public override string GetStatsText()
    {
        return "Level: " + GetTotalLevel() +
               "\nDamage: " + damage +
               "\nSplash Damage: " + splashDamage +
               "\nSplash Radius: " + splashRadius;
    }

    public override string GetUpgradeOptionAText()
    {
        if (upgradeALevel == 1)
        {
            return "+3 Damage, +1 Splash Damage";
        }
        else if (upgradeALevel == 2)
        {
            return "+6 Damage, +2 Splash Damage";
        }

        return "Maxed";
    }

    public override string GetUpgradeOptionBText()
    {
        if (upgradeBLevel == 1)
        {
            return "+ Splash Radius";
        }
        else if (upgradeBLevel == 2)
        {
            return "++ Splash Radius";
        }

        return "Maxed";
    }

    protected override void ApplyUpgradeA()
    {
        if (upgradeALevel == 1)
        {
            damage += 3f;
            splashDamage += 1f;
        }
        else if (upgradeALevel == 2)
        {
            damage += 6f;
            splashDamage += 2f;
        }
    }

    protected override void ApplyUpgradeB()
    {
        if (upgradeBLevel == 1)
        {
            splashRadius += 0.5f;
        }
        else if (upgradeBLevel == 2)
        {
            splashRadius += 1f;
        }
    }
}