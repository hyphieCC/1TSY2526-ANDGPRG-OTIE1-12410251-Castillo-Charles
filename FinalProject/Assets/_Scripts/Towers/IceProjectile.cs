using UnityEngine;

public class IceProjectile : Projectile
{
    float slowMultiplier;
    float slowDuration;

    public void SetIceProjectile(Enemy target, float slowMultiplier, float slowDuration)
    {
        this.target = target;
        this.slowMultiplier = slowMultiplier;
        this.slowDuration = slowDuration;
    }

    protected override void HitTarget()
    {
        if (target != null)
        {
            target.ApplySlow(slowMultiplier, slowDuration);
        }

        Destroy(gameObject);
    }
}