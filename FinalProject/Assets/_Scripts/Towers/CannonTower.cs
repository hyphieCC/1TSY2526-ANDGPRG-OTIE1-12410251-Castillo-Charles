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
        GameObject projectileObj = Instantiate(cannonProjectilePrefab, shootPoint.position, shootPoint.rotation);
        CannonProjectile cannonProjectile = projectileObj.GetComponent<CannonProjectile>();
        cannonProjectile.SetCannonProjectile(target, damage, splashRadius, splashDamage);
    }
}