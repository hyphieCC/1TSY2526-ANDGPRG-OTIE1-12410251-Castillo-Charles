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
}