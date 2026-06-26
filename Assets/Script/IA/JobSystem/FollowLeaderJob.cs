using UnityEngine;
using UnityEngine.AI;

public class FollowLeaderJob : AIJob
{
    public bool canFollow;
    public Health leaderHealth;
    public NavMeshAgent agent;

    public FollowLeaderJob(
        AIController controller,
        Health leaderHealth_,
        NavMeshAgent agent_
        )
        : base(controller)
        
    {
        leaderHealth = leaderHealth_;
        agent = agent_;
    }

    public override float GetPriority()
    {
        return 10;
    }

    public override bool CanRun()
    {
        return leaderHealth != null && !leaderHealth.isDead;
    }

    public override void Start()
    {
        agent.ResetPath();
        canFollow = true;
    }

    public override void Tick()
    {
        return;
    }

    public override void Stop()
    {
        canFollow = false;
        agent.ResetPath();
    }
}
