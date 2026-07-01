using UnityEngine;
using UnityEngine.AI;

public class AttackEnemyJob : AIJob
{
    private CombatTargeting targeting;
    private CombatBehaviour combat;
    private NavMeshAgent agent;

    public AttackEnemyJob(AIController controller)
        : base(controller)
    {
        targeting =
            controller.GetComponent<CombatTargeting>();

        combat =
            controller.GetComponent<CombatBehaviour>();

        agent = 
            controller.GetComponent<NavMeshAgent>();
    }

    public override bool CanRun()
    {
        fleeEnemyJob job = controller.jobs.Find(x => x is fleeEnemyJob) as fleeEnemyJob;

        targeting.UpdateTarget();
        
        if(job != null)
        {
            Debug.LogError("ne peut pas assignier fleeEnemyJob et AttackEnemyJob");
        }

        return job == null && targeting.HasTarget;
    }

    public override float GetPriority()
    {
        return targeting.HasTarget ? 1000 : 0;
    }

    public override void Start()
    {
        agent.ResetPath();
        combat.StartCombat();
        isStopped = false;
    }

    private float timer;
    private bool isStopped;
    public override void Tick()
    {
        timer += Time.deltaTime;

        if (timer >= 0.2f && CanRun() && !isStopped)
        {
            timer = 0f;
            targeting.UpdateTarget();
        }
    }

    public override void Stop()
    {
        agent.ResetPath();
        combat.StopCombat();
        isStopped = true;
    }
}