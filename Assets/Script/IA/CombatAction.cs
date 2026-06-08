using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CombatAction : MonoBehaviour
{
    [Header("Stats")]
    public float health;

    //ne pas modifier les damages car ils sont calculer celon les states
    private float damage;
    public float attackStat;
    public float strenght;
    public int defenceStat;
    public float cooldown;
    public HumanoidStates humanoidState;
    public HumainState humainState;
    public float fov;
    private float range;
    public NavMeshAgent agent;
    [SerializeField]private Stamina stamina;
    private float perfectBlockTiming = 0.2f;
    public  float regenRate = 10;

    [Header("Enemy")]
    public GameObject ennemy;
    private MeleeCombatIA enemyScript;
    private HumainState enemyState;
    private Creature ennemyCreature;
    public Health ennemyHealth;
    private Stamina ennemyStamina;
    [SerializeField] private GameObject weapon;
    private bool isFleing;

    
    public event Action OnAttack;
    public event Action OnParry;
    public event Action OnStun;

    float nextActionTime = 0f;

    public Collider[] buffer = new Collider[100];
    [SerializeField] private LayerMask detectionLayer;

    private float nextPathUpdate;

    public int attackersCount = 0;
    public int maxAttackers = 2;
    private bool isResetting = false;
    
    public void UpdateStates()
    {
        attackStat = humanoidState.attackStat;
        defenceStat = humanoidState.defenceStat;
        cooldown = humanoidState.cooldown;
        range = humanoidState.range;
        stamina.maxValue = humanoidState.endurance * 5;
        strenght = humanoidState.strenght;
    }
    public bool CanBeTargeted()
    {
        return attackersCount < maxAttackers;
    }

    public void LookAtEnnemy()
    {
        if (ennemy == null) return;

        Vector3 dir = ennemy.transform.position - transform.position;
        dir.y = 0;

        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10f * Time.deltaTime);
    }
    public void TrySetEnemy(GameObject newEnemy)
    {
        CombatAction targetCombat = newEnemy.GetComponent<CombatAction>();

        if (targetCombat == null) return;

        if (!targetCombat.CanBeTargeted()) return;

        SetEnemy(newEnemy);
        targetCombat.attackersCount++;
    }

    public void SetEnemy(GameObject newEnemy)
    {
        if(ennemy == newEnemy)return;

        if (ennemyHealth != null)
            ennemyHealth.OnDeath -= ResetEnemy;
            
        ennemy = newEnemy;
        enemyScript = ennemy.GetComponent<MeleeCombatIA>();
        enemyState = ennemy.GetComponent<HumainState>();
        ennemyCreature = ennemy.GetComponent<Creature>();
        ennemyHealth = ennemy.GetComponent<Health>();
        ennemyStamina = ennemy.GetComponent<Stamina>();

        humainState.hasReactedToAttack = false;
            if (ennemyHealth != null)
        ennemyHealth.OnDeath += ResetEnemy;
    
    if (ennemy != null)
    {
        CombatAction old = ennemy.GetComponent<CombatAction>();
        if (old != null)
            old.attackersCount = Mathf.Max(0, old.attackersCount - 1);
    }

    

    if (ennemy == null)
        return;

    enemyScript = ennemy.GetComponent<MeleeCombatIA>();
    enemyState = ennemy.GetComponent<HumainState>();
    ennemyCreature = ennemy.GetComponent<Creature>();
    ennemyHealth = ennemy.GetComponent<Health>();
    ennemyStamina = ennemy.GetComponent<Stamina>();

    humainState.hasReactedToAttack = false;
    }
    
    public void ResetEnemy()
{
    if (isResetting) return;
        isResetting = true;

    if (ennemy != null)
{
    CombatAction targetCombat = enemyScript.combatAction;
    if (targetCombat != null)
        targetCombat.attackersCount--;
}
    ennemy = null;
    ennemyHealth = null;
    enemyScript = null;
    enemyState = null;
    ennemyCreature = null;
    print("kirk" + gameObject.name);

    if (agent != null && agent.isOnNavMesh)
    {
        agent.ResetPath();
    }
    

    humainState.isAttacking = false;
    humainState.isThinking = false;
    isResetting = false;
}
    public void Regenerate()
    {
        stamina.addStamina(regenRate * Time.deltaTime);
    }



public void HandleMovement(float dist)
{
    if (humainState.goToEnnemy || agent == null || ennemy == null || !agent.isOnNavMesh)
        return;

    if (dist > range && !isFleing)
    {
        agent.isStopped = false;
        if (Time.time >= nextPathUpdate)
        {
            nextPathUpdate = Time.time + 0.2f;

            agent.SetDestination(ennemy.transform.position);
        }
        
        agent.isStopped = dist <= range;
    }
    else
    {
        agent.isStopped = true;
    }
}

    public void HandleCombat(float dist)
    {
        if (humainState.isThinking) return;
        if (Time.time < nextActionTime) return;
        if (humainState.isAttacking || humainState.isParring || humainState.isStuned) return;

        if (dist <= range)
        {
            StartCoroutine(ThinkAndAct());
        }
    }

    IEnumerator ThinkAndAct()
    {
        humainState.isThinking = true;
        
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.5f));
        // modifier k pour modifier la difficulter a mettre un coup
        float k = 0.8f;
        int rand = UnityEngine.Random.Range(0, 100);
        float chance = MathF.Pow(attackStat / 100, k) * 100f;
        

        if (rand < Mathf.Max(chance, 25f))
        {
            StartCoroutine(AttackRoutine());
        }


        nextActionTime = Time.time + UnityEngine.Random.Range(0.5f, 1.2f);

        humainState.isThinking = false;
    }


    private IEnumerator AttackRoutine()
    {
        if(stamina.value < 20) yield break;
        if(ennemyHealth != null)
        {
            if(ennemyHealth.isDead) yield break;            
        }
        stamina.RemoveStamina(20);
        humainState.isAttacking = true;
        OnAttack?.Invoke();

        if (agent != null && agent.enabled == true)
            agent.isStopped = true;
        
        //previent l'ia adverse que qu'elle est attaqué
        if (enemyScript != null && ennemyHealth.isDead == false)
        {
            enemyScript.HandleEnemyReaction();
            ennemy.GetComponent<CombatAction>().TrySetEnemy(gameObject);
        }

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.4f));

        if (ennemy != null && ennemyCreature != null)
        {
            float dist = Vector3.Distance(transform.position, ennemy.transform.position);

            // sécurité importante
            if (dist <= range)
            {
                //si l'ennemy Parry alors un stune est appliqué
                if(enemyState.perfecBlock)
                {
                    humainState.isStuned = true;
                    stamina.RemoveStamina(30);
                    OnStun?.Invoke();
                    StartCoroutine(ResetStune(UnityEngine.Random.Range(0.2f, 0.4f)));
                    if(ennemyStamina != null)
                    {
                        ennemyStamina.addStamina(16);
                    }
                    
                } else if (enemyState.isParring)
                {
                    humainState.isStuned = true;
                    StopCoroutine("ResetStune");
                    StartCoroutine(ResetStune(UnityEngine.Random.Range(0.2f, 0.4f)));
                    stamina.RemoveStamina(20);

                    OnStun?.Invoke();
                    if (ennemyStamina != null)
                    {
                        ennemyStamina.RemoveStamina(10);
                    }
                    
                    
                } else
                {
                    //Fais la moyenne des state d'attaque de l'ia et de l'arme auquel elle a joute 5 pour que meme si la state est basse l'ia puisse faire des degats ajoute le bonus d'attaque 
                    //et multiple pour faire des coup critique ou moins efficase
                    float s = strenght / 100f;
                    float a = attackStat / 100f;
                    float w = weapon.GetComponent<WeaponState>().damage / 100f;
                    damage = ((s * 0.3f) + (a * 0.4f) + (w * 0.3f)) *100;
                    float damageReduction = (humanoidState.strenght/100 * 0.4f + humanoidState.defenceStat/100 * 0.6f)*50;
                    print(damageReduction);
                    ennemyHealth.TakeDamage(Mathf.FloorToInt(damage - damageReduction), ennemyHealth.GetRandomBodyPart());
                }
            }
        }

        yield return new WaitForSeconds(cooldown);

        humainState.isAttacking = false;

        if (agent != null && agent.enabled == true)
            agent.isStopped = false;

    }
    

    public int GetSourondingEnemiesNonAlloc()
    {
        return Physics.OverlapSphereNonAlloc(transform.position, fov, buffer, detectionLayer);
        
    }
    
    public void HandleEnemyReaction()
    {
        if (!humainState.hasReactedToAttack && !humainState.isParring && !humainState.isStuned)
        {
            humainState.hasReactedToAttack = true;
            StartCoroutine(ReactToAttack());
        }
    }

    private IEnumerator ReactToAttack()
    {
        //simule les temps de reaction humain
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.25f));
        TryParring();
    }

    private void TryParring()
    {
        int chance = UnityEngine.Random.Range(0, 100);

        if (!humainState.isParring && !humainState.isStuned && chance <= defenceStat + 10)
        {
            StartCoroutine(Parry());
        }
    }

    private IEnumerator Parry()
    {
        stamina.RemoveStamina(10);
        humainState.perfecBlock = true;
        OnParry?.Invoke();
        yield return new WaitForSeconds(perfectBlockTiming);
        humainState.perfecBlock = false;
        humainState.isParring = true;
        yield return new WaitForSeconds(0.5f);
        humainState.isParring = false;
    }
void Start()
{
    UpdateStates();
    agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
    agent.avoidancePriority = UnityEngine.Random.Range(20, 80);

    agent.stoppingDistance = range;
    if (range <= 0)
        range = 2f;

}

    IEnumerator ResetStune(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        humainState.isStuned = false;
    }
}
