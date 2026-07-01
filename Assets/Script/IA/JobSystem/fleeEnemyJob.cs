using UnityEngine;
using UnityEngine.AI;

public class fleeEnemyJob : AIJob
{
    private CombatTargeting targeting;
    private NavMeshAgent agent;

    public fleeEnemyJob(AIController controller)
        : base(controller)
    {
        targeting = controller.GetComponent<CombatTargeting>();
        agent = controller.GetComponent<NavMeshAgent>();
    }


    public override bool CanRun()
    {
        AttackEnemyJob job =
            controller.jobs.Find(x => x is AttackEnemyJob) as AttackEnemyJob;

        if(job != null)
            return false;

        return targeting.HasTarget;
    }


    public override float GetPriority()
    {
        return targeting.HasTarget ? 2000 : 0;
    }


    public override void Start()
    {
        agent.ResetPath();
    }


    private float timer;
    private float fleeRate = 0.5f;
    private float fleeDistance = 10f;


    public override void Tick()
    {
        if(!targeting.HasTarget)
            return;


        timer += Time.deltaTime;

        if(timer < fleeRate)
            return;

        timer = 0;


        Vector3 enemyPos =
            targeting.CurrentTarget.transform.position;


        Vector3 fleeDirection =
    (controller.transform.position - enemyPos).normalized;

        // ajoute un angle aléatoire
        float randomAngle = Random.Range(-45f, 45f);

        fleeDirection = Quaternion.Euler(0, randomAngle, 0) * fleeDirection;


        Vector3 targetPosition =
            controller.transform.position + fleeDirection * (fleeDistance * Random.Range(0.8f, 1.2f));


        if(NavMesh.SamplePosition(
            targetPosition,
            out NavMeshHit hit,
            5f,
            NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }


    public override void Stop()
    {
        agent.ResetPath();
    }
}