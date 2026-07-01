using UnityEngine;
using UnityEngine.AI;

public class Workjob : AIJob
{
    //float timer;
    //float tickRate = 0.5f;


    private NeedsComponent needs;
    private GeneriqueWorkStation workStation;
    private NavMeshAgent agent;

    public Workjob(
        AIController controller,
        NeedsComponent needs,
        NavMeshAgent agent)
        : base(controller)
    {

        this.needs = needs;
        this.agent = agent;

        GeneriqueWorkStation[] stations = controller.GetComponent<Creature>().citie.workStations;

        if(stations.Length < 1)
        {
            Debug.LogError("la liste de workstation est a 0");
        }
        
        foreach (var station in stations)
        {
            if (station != null && station.owner == null && !station.isUsed)
            {
                workStation = station;
                workStation.owner = controller.GetComponent<Creature>();
                break;
            }
        }

        if(workStation == null)
        {
            Debug.LogError("pas de workStation assignier");
        }
    }

    public override float GetPriority()
    {
        return needs.energie;
    }

    public override bool CanRun()
    {
        return needs.energie > 50  && workStation != null && workStation.workPoints.Length > 0;
    }

    public override void Start()
    {
        agent.ResetPath();
        agent.SetDestination(workStation.workPoints[Random.Range(0, workStation.workPoints.Length)].position);
    }

    public override void Tick()
    {
        if(agent.remainingDistance < agent.stoppingDistance)
        {
            if (!workStation.isUsed)
            {
                workStation.workParicle.SetActive(true);
                controller.transform.rotation = Quaternion.Euler(-workStation.transform.forward);
                workStation.isUsed = true;
            }
        }

    }

    public override void Stop()
    {
        agent.ResetPath();
        workStation.workParicle.SetActive(false);
        workStation.isUsed = false;
    }
}