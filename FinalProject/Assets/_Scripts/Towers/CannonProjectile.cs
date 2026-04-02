using UnityEngine;

public class CannonProjectile : Projectile
{
    float splashRadius;
    float splashDamage;

    public void SetCannonProjectile(Enemy target, float damage, float splashRadius, float splashDamage)
    {
        this.target = target;
        this.damage = damage;
        this.splashRadius = splashRadius;
        this.splashDamage = splashDamage;
    }

    protected override void HitTarget()
    {
        if (target != null && target.GetEnemyType() == EnemyType.Ground)
        {
            target.TakeDamage(damage);
        }

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == target)
            {
                continue;
            }

            if (enemies[i].GetEnemyType() != EnemyType.Ground)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, enemies[i].transform.position);

            if (distance <= splashRadius)
            {
                enemies[i].TakeDamage(splashDamage);
            }
        }

        Destroy(gameObject);
    }
}