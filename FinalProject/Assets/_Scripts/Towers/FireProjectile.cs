using UnityEngine;

public class FireProjectile : Projectile
{
    float burnDamagePerTick;
    float burnDuration;
    float burnTickInterval;

    public void SetFireProjectile(Enemy target, float burnDamagePerTick, float burnDuration, float burnTickInterval)
    {
        this.target = target;
        this.burnDamagePerTick = burnDamagePerTick;
        this.burnDuration = burnDuration;
        this.burnTickInterval = burnTickInterval;
    }

    protected override void HitTarget()
    {
        if (target != null)
        {
            target.ApplyBurn(burnDamagePerTick, burnDuration, burnTickInterval);
        }

        Destroy(gameObject);
    }
}