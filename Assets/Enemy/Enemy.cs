using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float wanderRadius = 8f;
    public float wanderInterval = 3f;

    private NavMeshAgent agent;
    private float wanderTimer;
    private Vector3 originPos;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originPos = transform.position;
        wanderTimer = wanderInterval;
    }

    void Update()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderInterval)
        {
            Vector3 newPos = RandomNavSphere(originPos, wanderRadius, NavMesh.AllAreas);
            agent.SetDestination(newPos);
            wanderTimer = 0f;
        }
    }

    // Genera una posición aleatoria válida en el NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
