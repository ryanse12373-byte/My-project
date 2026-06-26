using UnityEngine;

[CreateAssetMenu(menuName = "AI/Jobs/WanderAroundCitie")]
public class WanderAroundCitieJobFactory : JobFactorySO
{
    public override AIJob Create(AIController controller)
    {

        return new WanderAroundCitieJob(
            controller);
              
    }
}
