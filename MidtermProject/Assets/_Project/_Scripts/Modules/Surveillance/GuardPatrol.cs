using UnityEngine;
using UnityEngine.AI;

public class GuardPatrol : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float waitAtPointSeconds = 0.5f;

    [Header("NavMesh Sampling")]
    [Tooltip("How far we’re allowed to nudge a waypoint onto the NavMesh.")]
    [SerializeField] private float sampleRadius = 1.0f;

    private NavMeshAgent agent;
    private int index;
    private float waitTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("GuardPatrol: No waypoints assigned.");
            enabled = false;
            return;
        }

        index = 0;
        SetDestinationToWaypoint(index);
    }

    private void Update()
    {
        if (agent == null) return;
        if (agent.pathPending) return;

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Advance();
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance + 0.05f)
        {
            if (agent.hasPath && agent.velocity.sqrMagnitude > 0.01f) return;

            waitTimer += Time.deltaTime;
            if (waitTimer >= waitAtPointSeconds)
            {
                waitTimer = 0f;
                Advance();
            }
        }
    }

    private void Advance()
    {
        index = (index + 1) % waypoints.Length;
        SetDestinationToWaypoint(index);
    }

    private void SetDestinationToWaypoint(int i)
    {
        if (agent == null || waypoints[i] == null) return;

        Vector3 target = waypoints[i].position;

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
            target = hit.position;

        agent.isStopped = false;
        agent.SetDestination(target);
    }
}
