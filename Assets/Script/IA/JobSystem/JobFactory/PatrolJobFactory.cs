using UnityEngine;

[CreateAssetMenu(menuName = "AI/Jobs/Patrol")]
public class WaypointPatrolJobFactory : JobFactorySO
{
    public override AIJob Create(AIController controller)
    {
        PointPatrolTemp context =
            controller.GetComponent<PointPatrolTemp>();

        if (context == null)
            return null;
        
        Debug.Log(context.name);

        if (context.points.Count < 1)
            return null;

        return new WaypointPatrolJob(
            controller,
            context.points);
              
    }
}