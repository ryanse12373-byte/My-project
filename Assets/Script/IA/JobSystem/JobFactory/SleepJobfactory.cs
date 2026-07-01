using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Jobs/Sleep")]
public class SleepJobfactory : JobFactorySO
{
    public override AIJob Create(AIController controller)
    {
        NavMeshAgent agent = 
            controller.GetComponent<NavMeshAgent>();

        NeedsComponent needs =
            controller.GetComponent<NeedsComponent>();

        if(agent == null )
        {
            Debug.LogError("Agent est null");
            return null;
        }

        if(needs == null)
        {
            Debug.LogError("needs est null");
            return null;
        }

        return new SleepJob(
            controller,
            needs,
            agent
        );
    }
}
