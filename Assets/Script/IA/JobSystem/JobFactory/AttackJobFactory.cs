using UnityEngine;

[CreateAssetMenu(menuName = "AI/Jobs/Attack")]
public class AttackJobFactory : JobFactorySO
{
    public override AIJob Create(AIController controller)
    {
        return new AttackEnemyJob(controller);
    }
}