using UnityEngine;
using UnityEngine.AI;

public class SleepJob : AIJob
{
    private NeedsComponent needs;
    private BedWorkStation AIbed;
    private NavMeshAgent agent;

    public SleepJob(
        AIController controller,
        NeedsComponent needs,
        NavMeshAgent agent)
        : base(controller)
    {

        this.needs = needs;
        this.agent = agent;

        BedWorkStation[] beds = controller.GetComponent<Creature>().citie.bedWorkStations;

        if(beds == null || beds.Length < 1)
        {
            Debug.LogError("pas de lit disponible");
            return;
        }
        
        foreach (var bed in beds)
        {
            if (bed != null && bed.owner == null && !bed.isUsed)
            {
                AIbed = bed;
                bed.owner = controller.GetComponent<Creature>();
                break;
            }
        }
    }

    public override float GetPriority()
    {
        return 100 - needs.energie;
    }

    public override bool CanRun()
    {
        return (needs.energie < 20 || needs.isResting) && AIbed != null && AIbed.workPoints.Length > 0 && agent != null;
    }

    public override void Start()
    {
        agent.enabled = true;
        agent.ResetPath();
        agent.SetDestination(AIbed.workPoints[Random.Range(0, AIbed.workPoints.Length)].position);
    }
    
    bool hasArrived = false;
    public override void Tick()
    {
        if(hasArrived)
            return;

        if(!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            hasArrived = true;

            agent.enabled = false;

            AIbed.sleepParticle.SetActive(true);

            controller.transform.position =
                AIbed.workPoints[0].position;

            controller.transform.rotation = Quaternion.identity;

            controller.transform.rotation = Quaternion.Euler(-90, 0, 0);

            needs.isResting = true;
        }
    }

    public override void Stop()
    {
        agent.enabled = true;
        AIbed.sleepParticle.SetActive(false);
        needs.isResting = false;
        agent.ResetPath();
        AIbed.isUsed = false;
    }
}
