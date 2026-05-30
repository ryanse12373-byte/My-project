using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private bool canDoAction = true;
    [SerializeField] private HumainState playerState;
    [SerializeField] private HumanoidStates states;
    [SerializeField] private WeaponState weaponState;
    [SerializeField] private Animator weaponAnim;
    [SerializeField]private Transform playerOrientation;
    [SerializeField] private Stamina stamina;
    
    private float range = 4f;
    private float perfectBlockTiming =0.15f;
    private float regenRate = 5;

    void Update()
    {
        HandleInput();
        Regenerate();
        
    }
    void OnCollisionEnter(Collision col)
{
    Debug.Log("Hit: " + col.gameObject.name);
}

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F)&& canDoAction)
        {
            StartCoroutine(Parry());
        }
        if(Input.GetMouseButtonDown(0)&& canDoAction)
        {
            StartCoroutine(Attack());
        }
    }
    private void Regenerate()
    {
        stamina.addStamina(regenRate * Time.deltaTime);
    }
    

    IEnumerator Parry()
    {
        if(stamina.value < 10) yield break;
        stamina.RemoveStamina(10);
        canDoAction = false;
        weaponAnim.SetTrigger("Parry");
        playerState.perfecBlock = true;
        yield return new WaitForSeconds(perfectBlockTiming);
        playerState.perfecBlock = false;
        playerState.isParring = true;
        StartCoroutine(ResetParry(0.5f));
    }

    private IEnumerator ResetParry(float time)
    {
        yield return new WaitForSeconds(time);
        playerState.isParring = false;
        canDoAction = true;
    }

    private IEnumerator Attack()
    {
        if(stamina.value < 20) yield break;
        stamina.RemoveStamina(20);
        RaycastHit hit;

        if (!Physics.Raycast(playerOrientation.position, playerOrientation.forward, out hit, range))
        yield break;

        if (!hit.collider.TryGetComponent(out Creature creature))
        yield break;

        canDoAction = false;

        yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));

     weaponAnim.SetTrigger("Attack");

        MeleeCombatIA enemy = hit.collider.GetComponent<MeleeCombatIA>();
        if (enemy != null && enemy.GetComponent<Health>().isDead == false)
            enemy.HandleEnemyReaction();
        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));

        HumainState enemyState = hit.collider.GetComponent<HumainState>();

        if (enemyState != null && enemyState.perfecBlock)
        {
            stamina.RemoveStamina(30);
            Stamina ennemyStam = enemy.GetComponent<Stamina>();
            if(ennemyStam != null)
            {
                ennemyStam.addStamina(16);
            }
            StartCoroutine(ResetDoAction(0.5f));
            yield break;
        }

        if (enemyState != null && enemyState.isParring)
        {
            stamina.RemoveStamina(10);
            Stamina ennemyStam = enemy.GetComponent<Stamina>();
            if(ennemyStam != null)
            {
                ennemyStam.RemoveStamina(10);
            }
            StartCoroutine(ResetDoAction(0.5f));
            yield break;
        }

        float damage = (states.attackStat + weaponState.damage) / 2f;
        Health health = hit.collider.GetComponent<Health>();

        // touche une partie du corp randome celon la taille de la partie


        health.TakeDamage(Mathf.FloorToInt(damage), health.GetRandomBodyPart());

        StartCoroutine(ResetDoAction(0.5f));
    }

    private IEnumerator ResetDoAction(float time)
    {
        yield return new WaitForSeconds(time);
        canDoAction = true;

    }
}
