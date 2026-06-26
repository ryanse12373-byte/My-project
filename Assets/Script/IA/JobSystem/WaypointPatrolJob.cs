using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrolJob : AIJob
{
    public List<Transform> patrolPoints;

    private readonly NavMeshAgent agent;

    private int currentIndex;

    private float waitTimer;

    private bool waiting;

    public WaypointPatrolJob(
        AIController controller,
        List<Transform> patrolPoints)
        : base(controller)
    {
        this.patrolPoints = patrolPoints;

        agent = controller.GetComponent<NavMeshAgent>();
    }

    public override bool CanRun()
    {
        return patrolPoints != null &&
               patrolPoints.Count > 0;
    }

    public override float GetPriority()
    {
        return 10;
    }

    public override void Start()
    {
        GoToCurrentPoint();
    }

    public override void Tick()
    {
        if (patrolPoints.Count == 0)
            return;

        if (waiting)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0)
            {
                waiting = false;

                currentIndex++;

                if (currentIndex >= patrolPoints.Count)
                    currentIndex = 0;

                GoToCurrentPoint();
            }

            return;
        }

        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            waiting = true;

            waitTimer =
                Random.Range(0, 0);
        }
    }

    public override void Stop()
    {
        if (agent != null)
            agent.ResetPath();
    }

    private void GoToCurrentPoint()
    {
        if (patrolPoints.Count == 0)
            return;

        agent.SetDestination(
            patrolPoints[currentIndex].position);
    }
}