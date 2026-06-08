using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehavoirTest : MonoBehaviour
{
    
    public NavMeshAgent agent;

    [Header("Patrol Settings")]
    public float patrolRadius = 10f;
    public float waitTime = 2f;
    public float minDistanceToPoint = 1f;

    private Vector3 startPosition;
    private bool isWaiting;

    [SerializeField] private Animator animator;

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(PatrolLoop());
    }

    IEnumerator PatrolLoop()
    {
        while (true)
        {
            if (!isWaiting)
            {
                Vector3 randomPoint = GetRandomPoint();

                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    animator.Play("walk");
                    isWaiting = true;
                }
            }

            // attendre d'arriver
            if (!agent.pathPending && agent.remainingDistance <= minDistanceToPoint)
            {
                yield return new WaitForSeconds(waitTime);
                isWaiting = false;
            }

            yield return null;
        }
    }

    Vector3 GetRandomPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * patrolRadius;
        randomDir += startPosition;
        randomDir.y = transform.position.y;
        return randomDir;
    }
}
