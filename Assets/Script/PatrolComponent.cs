using UnityEngine;
using UnityEngine.AI;

public class PatrolComponent : MonoBehaviour
{

    private float range = 10;
    [SerializeField] private NavMeshAgent agent;
    private PatrolType patrolType;
    private Transform[] waypoints;
    
    private Vector3 GetRandomPos()
    {
        Vector3 randomDir = Random.insideUnitSphere * range;
        randomDir += transform.position;
        return randomDir;
    }
    void Start()
    {
        
    }

    public void RandomPatrol()
    {
        NavMeshHit hit;
        if(NavMesh.SamplePosition(GetRandomPos(), out hit, range, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogError(gameObject.name + "n'a pas reussi à patrouiller");
        }
    }

    private void WaypointsPatrol()
    {
        if(waypoints.Length <= 1)
        {
            Debug.LogError("pas assez de waypoints");
            return;
        }

        
        
        
    }

    private enum PatrolType
    {
        random,
        waypoints
    }

}
