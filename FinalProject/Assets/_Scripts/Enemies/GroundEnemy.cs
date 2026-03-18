using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : Enemy
{
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyType = EnemyType.Ground;
    }

    public override void InitMonster(Vector3 targetPoint, float hpMultiplier)
    {
        base.InitMonster(targetPoint, hpMultiplier);
        //Debug.Log("GroundEnemy initialized");
        //Debug.Log("Going to: " + targetPoint);
        agent.SetDestination(targetPoint);
    }
}