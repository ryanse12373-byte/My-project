using UnityEngine;
using UnityEngine.AI;

public class WanderAroundCitieJob : AIJob
{
    private NavMeshAgent agent;

    private Cities citie;

    public WanderAroundCitieJob(AIController controller)
        : base(controller)
    {
        agent = 
            controller.GetComponent<NavMeshAgent>();

        citie =
            controller.GetComponent<Creature>().citie;

    }

    public override bool CanRun()
    {
        if(citie == null)
        {
            Debug.LogError("donne le job WanderAroundCities alors que le " + controller.GetComponent<Creature>().name + " n'a pas de ville assignier");
        }

        return citie != null && agent != null;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void Start()
    {
        agent.ResetPath();
        agent.SetDestination(citie.GetRandomPointInCitie());
    }


    public override void Tick()
    {
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(citie.GetRandomPointInCitie());
        }
    }

    public override void Stop()
    {
        agent.ResetPath();
    }
}
