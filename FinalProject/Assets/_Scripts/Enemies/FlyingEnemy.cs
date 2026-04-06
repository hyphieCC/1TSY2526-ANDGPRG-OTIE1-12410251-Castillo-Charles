using UnityEngine;

public class FlyingEnemy : Enemy
{
    [Header("Flying Enemy Stats")]
    [SerializeField] float speed;
    [SerializeField] float reachDistance;

    Transform[] waypoints;
    int waypointIndex = 0;

    private void Awake()
    {
        enemyType = EnemyType.Flying;
    }

    public void SetWaypoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
    }

    public override void InitMonster(Vector3 targetPoint, float hpMultiplier)
    {
        base.InitMonster(targetPoint, hpMultiplier);
    }

    protected override void Update()
    {
        base.Update();

        Vector3 currentTarget;

        if (waypointIndex < waypoints.Length)
        {
            currentTarget = waypoints[waypointIndex].position;

            if (Vector3.Distance(transform.position, currentTarget) <= reachDistance)
            {
                waypointIndex++;
            }
        }
        else
        {
            currentTarget = targetPoint;
        }

        Vector3 direction = currentTarget - transform.position;

        if (direction.magnitude > 0.05f)
        {
            Vector3 moveDir = direction.normalized;
            transform.position += moveDir * speed * slowMultiplier * Time.deltaTime;
            transform.forward = moveDir;
        }
    }
}