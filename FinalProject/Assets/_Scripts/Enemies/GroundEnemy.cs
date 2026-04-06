using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : Enemy
{
    NavMeshAgent agent;
    [Header("Ground Enemy Stats")]
    [SerializeField] float baseMoveSpeed = 3.5f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyType = EnemyType.Ground;
    }

    protected override void Start()
    {
        base.Start();
        agent.speed = baseMoveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        agent.speed = baseMoveSpeed * slowMultiplier;
    }

    public override void InitMonster(Vector3 targetPoint, float hpMultiplier)
    {
        base.InitMonster(targetPoint, hpMultiplier);
        //Debug.Log("GroundEnemy initialized");
        //Debug.Log("Going to: " + targetPoint);
        agent.SetDestination(targetPoint);
    }
}