using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Jobs/fleeEnemy")]
public class fleeEnemyJobFactory : JobFactorySO
{
    public override AIJob Create(AIController controller)
    {
        return new fleeEnemyJob(
            controller);
    }
}
