using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class IABrain : MonoBehaviour
{
    [SerializeField] private CombatAction combatAction;
    [SerializeField] private PatrolComponent patrol;
    [SerializeField] private NavMeshAgent agent;
    private bool isPatroling;
    

private void Start()
{
    StartCoroutine(Tick());
}

private IEnumerator Tick()
{
    while (true)
    {
        
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
    {
        isPatroling = false;
    }
        PatrolManager();
        yield return new WaitForSeconds(0.2f); // fréquence du tick
        
    }
}

private void PatrolManager()
{
    if (combatAction.ennemy != null &&
        combatAction.ennemyHealth != null &&
        !combatAction.ennemyHealth.isDead)
    {
        isPatroling = false;
        return;
    }

    if (!isPatroling)
        Patrol();
}

    void Patrol()
{
    print("jeffrey kirk");
    isPatroling = true;
    patrol.RandomPatrol();
}
}
