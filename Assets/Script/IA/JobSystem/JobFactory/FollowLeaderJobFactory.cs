using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Jobs/FollowLeader")]
public class FollowLeaderJobFactory : JobFactorySO
{
    public override AIJob Create(AIController controller)
{
    PatrolLeaderContainer container = 
        controller.GetComponent<PatrolLeaderContainer>();

    if(container == null || container.leader == null)
    {
        Debug.LogError("Pas de leader trouvé pour " + controller.name);
        return null;
    }


    Health leaderHealth = 
        container.leader.GetComponent<Health>();

    NavMeshAgent agent = 
        controller.GetComponent<NavMeshAgent>();


    if(leaderHealth == null)
    {
        Debug.LogError("Leader sans Health : " + controller.name);
        return null;
    }


    return new FollowLeaderJob(
        controller,
        leaderHealth,
        agent
    );
}
}