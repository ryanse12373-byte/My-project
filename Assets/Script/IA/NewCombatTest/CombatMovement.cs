using UnityEngine;
using UnityEngine.AI;

public class CombatMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private CombatTargeting targeting;
    private CombatAttack attack;
    
    

    private float nextPathUpdate;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        targeting = GetComponent<CombatTargeting>();
        attack = GetComponent<CombatAttack>();
    }

    public void Tick()
    {
        if (!targeting.HasTarget)
            return;

        float sqrDist =
            (targeting.CurrentTarget.transform.position -
             transform.position).sqrMagnitude;

        float attackRange =
            attack.Range * attack.Range;

        if (sqrDist <= attackRange)
        {
            agent.isStopped = true;
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        if (Time.time < nextPathUpdate)
            return;

        nextPathUpdate =
            Time.time + Random.Range(0.4f, 0.8f);

        agent.SetDestination(
            targeting.CurrentTarget.transform.position);

    }

    public void FaceTarget()
    {
        if (!targeting.HasTarget)
            return;

        Vector3 dir =
            targeting.CurrentTarget.transform.position
            - transform.position;

        dir.y = 0;

        Quaternion targetRot =
            Quaternion.LookRotation(dir);

        transform.rotation =
            Quaternion.Lerp(
                transform.rotation,
                targetRot,
                10f * Time.deltaTime);
    }
}