using System;
using System.Collections;
using UnityEngine;

public class CombatAttack : MonoBehaviour
{
    private CombatTargeting targeting;
    private CombatStats stats;
    private CombatDefense defense;
    private CombatStateMachine stateMachine;
    public WeaponState weaponState;
    private Stamina stamina;
    public float Range => stats.Range;
    Creature selfCreature;

    public event Action OnAttack;
    public event Action OnStun;

    private float nextAttackTime;
    

    private void Awake()
    {
        selfCreature = GetComponent<Creature>();
        targeting = GetComponent<CombatTargeting>();
        stats = GetComponent<CombatStats>();
        stateMachine = GetComponent<CombatStateMachine>();
        stamina = GetComponent<Stamina>();
    }

    private void Start()
    {
        StartCoroutine(OneFrameDelay());
    }


    IEnumerator OneFrameDelay()
    {
        yield return new WaitForEndOfFrame();

        weaponState = GetComponentInChildren<WeaponState>();
        print("la fonction se lance");

    }

    public void Tick()
    {
        if (!targeting.HasTarget)
            return;

        if (Time.time < nextAttackTime)
            return;

        if (stateMachine.IsBusy)
            return;

        float dist =
            Vector3.Distance(
                transform.position,
                targeting.CurrentTarget.transform.position);

        if (dist > stats.Range)
            return;

        StartCoroutine(AttackRoutine());

        nextAttackTime =
            Time.time + stats.Cooldown;
    }

    IEnumerator AttackRoutine()
    {
        CombatTargeting targetTargeting =
            targeting.CurrentTarget.GetComponent<CombatTargeting>();


        if (targetTargeting != null)
        {
            targetTargeting.ForceTarget(selfCreature);
        }

        
        int chance = UnityEngine.Random.Range(0, 100);

        if (chance >= stats.Defence + 30)
            yield break;

        if (stamina.value < 20)
            yield break;
        
        OnAttack?.Invoke();

        stamina.RemoveStamina(20);

        stateMachine.CurrentState =
            CombatState.Attacking;

        CombatDefense targetDefense =
            targeting.CurrentTarget.GetComponent<CombatDefense>();
        

        targetDefense.TryParry();

        yield return new WaitForSeconds(
            UnityEngine.Random.Range(0.2f, 0.4f));

        DealDamage();

        /*yield return new WaitForSeconds(
            stats.Cooldown);*/

        stateMachine.CurrentState =
            CombatState.Idle;
    }

    void DealDamage()
    {
        Creature target =
            targeting.CurrentTarget;

        if (target == null)
            return;

        CombatDefense defense =
            target.GetComponent<CombatDefense>();

        if (defense != null)
        {
            if (defense.IsPerfectBlock)
            {
                StartCoroutine(Stun(0.4f));
                return;

            }

            if (defense.IsParrying)
            {
                StartCoroutine(Stun(0.25f));
                return;
            }
        }

        CombatStats enemyStats =
            target.GetComponent<CombatStats>();

        float damage =
            stats.CalculateDamage(
                weaponState);

        float reduction =
            enemyStats.CalculateReduction();

        int finalDamage =
            Mathf.Max(
                1,
                Mathf.RoundToInt(
                    damage - reduction));

        target.health.TakeDamage(
            finalDamage,
            target.health.GetRandomBodyPart());

        var targetBrain = target.GetComponent<CombatTargeting>();
        if (targetBrain != null)
        {
            targetBrain.ForceTarget(selfCreature);
        }
    }

    IEnumerator Stun(float duration)
    {
        OnStun?.Invoke();
        stateMachine.CurrentState =
            CombatState.Stunned;

        yield return new WaitForSeconds(duration);

        stateMachine.CurrentState =
            CombatState.Idle;
    }


}