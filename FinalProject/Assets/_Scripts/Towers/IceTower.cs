using UnityEngine;

public class IceTower : Tower
{
    [Header("Ice Stats")]
    [SerializeField] float slowMultiplier = 0.5f;
    [SerializeField] float slowDuration = 2f;

    [Header("Projectile")]
    [SerializeField] GameObject iceProjectilePrefab;
    [SerializeField] Transform shootPoint;

    protected override void Attack(Enemy target)
    {
        SoundManager.Instance.PlaySFX3D(SoundManager.SFXType.IceShoot, shootPoint.position);

        GameObject projectileObj = Instantiate(iceProjectilePrefab, shootPoint.position, shootPoint.rotation);
        IceProjectile iceProjectile = projectileObj.GetComponent<IceProjectile>();
        iceProjectile.SetIceProjectile(target, slowMultiplier, slowDuration);
    }

    public override string GetTowerName()
    {
        return "Ice Tower";
    }

    public override string GetStatsText()
    {
        return "Level: " + GetTotalLevel() +
               "\nSlow Multiplier: " + slowMultiplier +
               "\nSlow Duration: " + slowDuration +
               "\nRange: " + range;
    }

    public override string GetUpgradeOptionAText()
    {
        if (upgradeALevel == 1)
        {
            return "+ Stronger Slow";
        }
        else if (upgradeALevel == 2)
        {
            return "++ Stronger Slow";
        }

        return "Maxed";
    }

    public override string GetUpgradeOptionBText()
    {
        if (upgradeBLevel == 1)
        {
            return "+ Longer Slow";
        }
        else if (upgradeBLevel == 2)
        {
            return "++ Longer Slow";
        }

        return "Maxed";
    }

    protected override void ApplyUpgradeA()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SFXType.UpgradeSuccess);
        if (upgradeALevel == 1)
        {
            slowMultiplier -= 0.1f;
        }
        else if (upgradeALevel == 2)
        {
            slowMultiplier -= 0.15f;
        }

        if (slowMultiplier < 0.2f)
        {
            slowMultiplier = 0.2f;
        }
    }

    protected override void ApplyUpgradeB()
    {
        if (upgradeBLevel == 1)
        {
            slowDuration += 0.5f;
        }
        else if (upgradeBLevel == 2)
        {
            slowDuration += 1f;
        }
    }
}