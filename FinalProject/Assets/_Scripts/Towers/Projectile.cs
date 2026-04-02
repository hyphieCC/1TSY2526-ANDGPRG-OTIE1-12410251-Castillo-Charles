using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 12f;

    protected Enemy target;
    protected float damage;

    public void SetProjectile(Enemy target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.transform.position - transform.position;
        float distanceThisFrame = moveSpeed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.position += direction.normalized * distanceThisFrame;
        transform.forward = direction.normalized;
    }

    protected virtual void HitTarget()
    {
        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}