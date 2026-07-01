using UnityEngine;

public class CombatTargeting : MonoBehaviour
{
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] float visionAngle = 120f;
    [SerializeField] LayerMask detectionLayer;
    [SerializeField] LayerMask obstacleLayer;

    private Vector3 lastKnownPosition;
    private float memoryTime = 3f;
    private float lastSeenTimer;


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

    private bool IsTargetValid(Creature target)
{
    if (target == null) return false;
    if (target.health.isDead) return false;

    float dist = (target.transform.position - transform.position).sqrMagnitude;
    if (dist > detectionRadius * detectionRadius) return false;

    Vector3 dir = (target.transform.position - transform.position).normalized;

    float dot = Vector3.Dot(transform.forward, dir);
    if (dot < cosHalfAngle) return false;

    if (Physics.Raycast(transform.position + Vector3.up * 1.5f,
        dir,
        Mathf.Sqrt(dist),
        obstacleLayer))
    {
        return false;
    }

    return true;
}

public bool HasMemory =>
    CurrentTarget != null && lastSeenTimer > 0;

public Vector3 LastKnownPosition => lastKnownPosition;

    public void UpdateTarget()
{
    if (CurrentTarget != null)
    {
        if(IsTargetValid(CurrentTarget))
        {
            lastKnownPosition = CurrentTarget.transform.position;
            lastSeenTimer = memoryTime;
        }
        else
        {
            lastSeenTimer -= Time.deltaTime;

            if(lastSeenTimer <= 0)
            {
                ClearTarget();
            }
        }
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

    Creature bestTarget = null;
    float bestDist = float.MaxValue;

    Vector3 position = transform.position;
    Vector3 forward = transform.forward;

    // 🔴 1. On vérifie si la target actuelle est encore valide
    if (CurrentTarget != null)
    {
        bool stillValid =
            !CurrentTarget.health.isDead &&
            Vector3.Distance(CurrentTarget.transform.position, position) <= detectionRadius;

        if (!stillValid)
        {
            ClearTarget();
        }
        else
        {
            // on garde une légère priorité à la target actuelle
            bestTarget = CurrentTarget;
            bestDist = (CurrentTarget.transform.position - position).sqrMagnitude;
        }
    }

    // 🔴 2. scan des ennemis
    for (int i = 0; i < count; i++)
    {
        Creature candidate = buffer[i].GetComponent<Creature>();

        if (candidate == null)
            continue;

        if (candidate == selfCreature)
            continue;

        if (candidate.health.isDead)
            continue;

        if (candidate.faction == selfCreature.faction)
            continue;


        if(!IsEnemy(candidate))
            continue;
        

        Vector3 dir = candidate.transform.position - position;
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

        // 🔴 3. sélection du meilleur target
        if (sqrDist < bestDist)
        {
            bestDist = sqrDist;
            bestTarget = candidate;
        }
    }

    // 🔴 4. assignation intelligente
    if (bestTarget != null)
    {
        if (CurrentTarget != bestTarget)
        {
            CurrentTarget?.GetComponent<CombatTargetSlot>()?.RemoveAttacker();
        }

        CombatTargetSlot slot =
            bestTarget.GetComponent<CombatTargetSlot>();

        if (slot == null || slot.RegisterAttacker())
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