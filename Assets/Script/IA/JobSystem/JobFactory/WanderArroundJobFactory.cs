using UnityEngine;

[CreateAssetMenu(menuName = "AI/Jobs/WanderAround")]
public class WanderAroundJobFactory : JobFactorySO
{
    public override AIJob Create(AIController controller)
    {
        return new WanderAroundJob(
            controller);      
    }
}
