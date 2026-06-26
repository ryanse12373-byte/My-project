using UnityEngine;

[CreateAssetMenu(menuName = "AI/Jobs/Eat")]
public class EatJobFactory : JobFactorySO
{
    public override AIJob Create(AIController controller)
    {
        NeedsComponent needs = controller.GetComponent<NeedsComponent>();

        return new EatJob(
            controller,
            needs);
    }
}