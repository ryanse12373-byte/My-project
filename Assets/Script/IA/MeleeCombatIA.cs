
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

    [SerializeField] private float forgetDistance = 15f;
    [SerializeField] private float recheckTime = 0.5f;

    private float recheckTimer;

    [SerializeField] private float activeDistance = 25f;
    private Transform player;
    

    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        creature = GetComponent<Creature>();
        faction = creature.faction;
        
        tickSpeed = Random.Range(0.15f, 0.3f);

        combatAction.UpdateStates();

        if (combatAction.ennemy != null && !combatAction.ennemyHealth.isDead)
            combatAction.SetEnemy(combatAction.ennemy);

        faction = GetComponent<Creature>().faction;


    }

void Update()
{
    float distanceWithPlayer = Vector3.Distance(transform.position, player.position);
    combatAction.Regenerate();
    if (distanceWithPlayer > activeDistance)
    {
        combatAction.LookAtEnnemy();
    }

    if (humainState.isStuned)
        return;

    timer += Time.deltaTime;

    if (timer < tickSpeed)
        return;

    timer -= tickSpeed;

    int count = combatAction.GetSourondingEnemiesNonAlloc();

    GameObject closest = null;
    float closestDist = Mathf.Infinity;


    for (int i = 0; i < count; i++)
    {
        Collider col = combatAction.buffer[i];
        Creature creature = col.GetComponent<Creature>();

        if (creature == null || creature.gameObject == gameObject || creature.health.isDead)
            continue;

        // ignore same faction
        FactionSO myFaction = faction;

        // faction relation check
        bool isEnemy = false;

        if (creature.GetComponent<PlayerCombat>())
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
/*else
{
    recheckTimer += Time.deltaTime;

    if (recheckTimer >= recheckTime)
    {
        recheckTimer = 0f;

        float dist = Vector3.Distance(
            transform.position,
            combatAction.ennemy.transform.position
        );

        bool shouldSwitch =
            combatAction.ennemyHealth == null ||
            combatAction.ennemyHealth.bodyParts.Count == 0 ||
            dist > forgetDistance||
            combatAction.ennemyHealth.isDead;

        if (shouldSwitch && closest != null)
        {
            combatAction.SetEnemy(closest);
        }
    }
}*/
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


    public virtual void HandleEnemyReaction()
    {

    }

}
