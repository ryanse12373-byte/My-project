
using UnityEngine;

public class MeleeCombatIA : MonoBehaviour
{
    [Header("States")]
    public HumainState humainState;
    public CombatAction combatAction;

    [SerializeField] private GameObject weapon;
    private Creature creature;
    private FactionSO faction;
    

    private float timer = 0;
    private float tickSpeed = 0.2f;

    [SerializeField] private float angle;
    

    

    void Start()
    {
        creature = GetComponent<Creature>();
        faction = creature.faction;

        
        tickSpeed = Random.Range(0.15f, 0.3f);

        combatAction.UpdateStates();

        if (combatAction.ennemy != null && !combatAction.ennemyHealth.isDead)
            combatAction.TrySetEnemy(combatAction.ennemy);

        faction = GetComponent<Creature>().faction;


    }

void Update()
{
    combatAction.Regenerate();
   
    

    if (humainState.isStuned)
        return;

    timer += Time.deltaTime;

    if (timer < tickSpeed)
        return;

    timer -= tickSpeed;
    combatAction.LookAtEnnemy();

    int count = combatAction.GetSourondingEnemiesNonAlloc();

    GameObject closest = null;
    float closestDist = Mathf.Infinity;


    for (int i = 0; i < count; i++)
    {
        Collider col = combatAction.buffer[i];
        Creature creature = col.GetComponent<Creature>();

        if (creature == null || creature.gameObject == gameObject || creature.health.isDead)
            continue;

        float signedAngle = Vector3.Angle(transform.forward, col.transform.position - transform.position);

        if (signedAngle > angle * 0.5f)
            continue;

Vector3 dir = col.transform.position - transform.position;
float d = dir.magnitude;
dir /= d;

// obstacle check
if (Physics.Raycast(transform.position + Vector3.up * 1.5f, dir, out RaycastHit hit, d))
{
    if (hit.collider.gameObject != col.gameObject)
        continue;
}

        // ignore same faction
        FactionSO myFaction = faction;

        // faction relation check
        bool isEnemy = false;

        if (combatAction.ennemyCreature.isPlayer)
        {
            isEnemy = myFaction.playerRelation <= -50;
        }
        else
        {
            isEnemy = myFaction.enemies.Contains(creature.faction.id);
        }

        if (!isEnemy)
            continue;

        float dist = Vector3.Distance(transform.position, creature.transform.position);

        if (dist < closestDist)
        {
            closestDist = dist;
            closest = creature.gameObject;
        }
    }
if (combatAction.ennemy == null || combatAction.ennemyHealth.isDead)
{
    if (closest != null)
        combatAction.SetEnemy(closest);
}
    if (combatAction.ennemy == null|| combatAction.ennemyHealth.isDead)
        return;

    float distToEnemy =
        Vector3.Distance(transform.position, combatAction.ennemy.transform.position);

    
    if (combatAction.ennemy == null || combatAction.ennemyHealth.isDead)
    {
        combatAction.SetEnemy(null);
        return;
    }

    if (combatAction.ennemyHealth != null && combatAction.ennemyHealth.isDead)
    {
        combatAction.SetEnemy(null);
        return;
    }

    combatAction.HandleMovement(distToEnemy);
    combatAction.HandleCombat(distToEnemy);

    if (combatAction.ennemy == null || combatAction.ennemyHealth.isDead)
    {
        combatAction.agent.ResetPath();
        return;
    }
}

void OnDrawGizmosSelected()
{
    Gizmos.color = Color.yellow;

    // distance max
    Gizmos.DrawWireSphere(transform.position, combatAction != null ? combatAction.fov : 5f);

    float halfAngle = angle * 0.5f;

    Vector3 forward = transform.forward;

    Vector3 leftDir = Quaternion.Euler(0, -halfAngle, 0) * forward;
    Vector3 rightDir = Quaternion.Euler(0, halfAngle, 0) * forward;

    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, transform.position + leftDir * combatAction.fov);
    Gizmos.DrawLine(transform.position, transform.position + rightDir * combatAction.fov);

    // lignes intermédiaires (optionnel)
    Gizmos.color = new Color(1, 0.5f, 0);
    int steps = 10;

    for (int i = 0; i <= steps; i++)
    {
        float t = i / (float)steps;
        float currentAngle = Mathf.Lerp(-halfAngle, halfAngle, t);

        Vector3 dir = Quaternion.Euler(0, currentAngle, 0) * forward;
        Gizmos.DrawLine(transform.position, transform.position + dir * combatAction.fov);
    }
}


    public virtual void HandleEnemyReaction()
    {

    }

}
