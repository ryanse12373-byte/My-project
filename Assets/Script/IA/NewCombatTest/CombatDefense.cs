using System;
using System.Collections;
using UnityEngine;

public class CombatDefense : MonoBehaviour
{
    [SerializeField] private float perfectBlockTime = 0.2f;

    private CombatStats stats;
    private CombatStateMachine stateMachine;
    public event Action OnParry;
    private Stamina stamina;

    public bool IsParrying { get; private set; }
    public bool IsPerfectBlock { get; private set; }

    private void Awake()
    {
        stats = GetComponent<CombatStats>();
        stateMachine = GetComponent<CombatStateMachine>();
        stamina = GetComponent<Stamina>();
    }

    public void TryParry()
    {
        if (IsParrying)
            return;

        if (stateMachine.CurrentState == CombatState.Stunned)
            return;

        int chance = UnityEngine.Random.Range(0, 100);

        if (chance <= stats.Defence + 30)
        {
            StartCoroutine(ParryRoutine());
        }
        
    }

    IEnumerator ParryRoutine()
    {
        if(stamina.value < 10)
            yield break;

        OnParry?.Invoke();

        stamina.RemoveStamina(10);

        stateMachine.CurrentState =
            CombatState.Parrying;

        IsPerfectBlock = true;

        yield return new WaitForSeconds(perfectBlockTime);

        IsPerfectBlock = false;
        IsParrying = true;

        yield return new WaitForSeconds(0.5f);

        IsParrying = false;

        stateMachine.CurrentState =
            CombatState.Idle;
    }
}