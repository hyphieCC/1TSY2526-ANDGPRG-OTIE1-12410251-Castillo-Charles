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
        GameObject projectileObj = Instantiate(iceProjectilePrefab, shootPoint.position, shootPoint.rotation);
        IceProjectile iceProjectile = projectileObj.GetComponent<IceProjectile>();

        iceProjectile.SetIceProjectile(target, slowMultiplier, slowDuration);
    }
}
