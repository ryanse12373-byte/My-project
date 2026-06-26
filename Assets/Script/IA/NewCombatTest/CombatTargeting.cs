using UnityEngine;

public class CombatTargeting : MonoBehaviour
{
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] float visionAngle = 120f;
    [SerializeField] LayerMask detectionLayer;
    [SerializeField] LayerMask obstacleLayer;
    private Creature selfCreature;

    private readonly Collider[] buffer = new Collider[64];

    public Creature CurrentTarget ;

    public bool HasTarget =>
        CurrentTarget != null &&
        !CurrentTarget.health.isDead;

    private float cosHalfAngle;
    private float nextAggroTime;


    private void Awake()
    {
        selfCreature = GetComponent<Creature>();

        cosHalfAngle =
            Mathf.Cos(visionAngle * 0.5f * Mathf.Deg2Rad);
    }

    public void UpdateTarget()
    {
        if (CurrentTarget != null &&
            CurrentTarget.health.isDead)
        {
            ClearTarget();
        }

        FindTarget();
    }

    private void FindTarget()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            detectionRadius,
            buffer,
            detectionLayer);

        Debug.Log(name + " trouve colliders : " + count);

        Creature bestTarget = null;
        float bestDist = float.MaxValue;

        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        for (int i = 0; i < count; i++)
        {
            Creature candidate =
                buffer[i].GetComponent<Creature>();




            if (candidate == null)
                continue;

            if (candidate == selfCreature)
                continue;

            if (candidate.health.isDead)
                continue;

            if(candidate.faction == selfCreature.faction)
                continue;


                

            Vector3 dir =
                candidate.transform.position - position;

            float sqrDist = dir.sqrMagnitude;

            dir.Normalize();

            float dot = Vector3.Dot(forward, dir);

            if (dot < cosHalfAngle)
                continue;

            if (Physics.Raycast(
                position + Vector3.up * 1.5f,
                dir,
                Mathf.Sqrt(sqrDist),
                obstacleLayer))
            {
                continue;
            }

            /*if (!IsEnemy(candidate))
                continue;

            var slot = candidate.GetComponent<CombatTargetSlot>();
            if (slot == null || !slot.CanAcceptAttacker())
                continue;
            */

            if (sqrDist < bestDist)
            {
                bestDist = sqrDist;
                bestTarget = candidate;
            }
        }

        

        if(bestTarget != null)
        {
            CombatTargetSlot slot =
                bestTarget.GetComponent<CombatTargetSlot>();

            if(slot == null || slot.RegisterAttacker())
            {
                CurrentTarget = bestTarget;
            }
        }
    }

    private bool IsEnemy(Creature target)
    {
        if (target.isPlayer)
            return selfCreature.faction.playerRelation <= -50;

        return selfCreature.faction.enemies.Contains(target.faction.id);
    }

    public void ClearTarget()
    {
        if(CurrentTarget != null)
        {
            CombatTargetSlot slot =
                CurrentTarget.GetComponent<CombatTargetSlot>();

            if(slot != null)
                slot.RemoveAttacker();
        }

        CurrentTarget = null;
    }

    public void ForceTarget(Creature attacker)
    {
        if (Time.time < nextAggroTime) return;

        nextAggroTime = Time.time + 0.5f;
        if (attacker == null || attacker.health.isDead)
            return;

        if (CurrentTarget != null)
        {
            CurrentTarget.GetComponent<CombatTargetSlot>()?.RemoveAttacker();
        }

        CurrentTarget = attacker;
        CurrentTarget.GetComponent<CombatTargetSlot>()?.RegisterAttacker();
        //print(attacker.firstName + " a forcé l'agros avec " + selfCreature.firstName);
    }








    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 origin = transform.position + Vector3.up * 0.5f;

        // cercle de détection
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // direction centrale
        Vector3 forward = transform.forward;

        float halfAngle = visionAngle * 0.5f;

        int steps = 20;

        for (int i = 0; i <= steps; i++)
        {
            float angle = -halfAngle + (visionAngle * i / steps);
            Vector3 dir = Quaternion.Euler(0, angle, 0) * forward;

            Gizmos.DrawRay(origin, dir * detectionRadius);
        }
        
    }

}