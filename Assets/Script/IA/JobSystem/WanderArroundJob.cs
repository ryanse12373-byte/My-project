using UnityEngine;
using UnityEngine.AI;

public class WanderAroundJob : AIJob
{
    private NavMeshAgent agent;



    public WanderAroundJob(AIController controller)
        : base(controller)
    {
        agent = 
            controller.GetComponent<NavMeshAgent>();
    }

    public override bool CanRun()
    {
        return agent != null;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void Start()
    {
        agent.ResetPath();

        agent.SetDestination(GetRandomPoint(agent.transform.position , 25));
    }


    public override void Tick()
    {
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(GetRandomPoint(agent.transform.position , 25));
        }
    }

    public override void Stop()
    {
        agent.ResetPath();
    }

    public Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector2 randomCircle = Random.insideUnitCircle * radius;

        Vector3 randomPos = center + new Vector3(randomCircle.x, 0f, randomCircle.y);

        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return center;
    }

    
    
}
