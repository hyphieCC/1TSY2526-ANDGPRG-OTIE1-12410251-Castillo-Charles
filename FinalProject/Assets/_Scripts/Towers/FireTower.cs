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
}