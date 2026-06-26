using UnityEngine;

public class CombatStateMachine : MonoBehaviour
{
    public CombatState CurrentState;

    public bool IsBusy =>
        CurrentState == CombatState.Attacking ||
        CurrentState == CombatState.Stunned ||
        CurrentState == CombatState.Parrying;
}

public enum CombatState
{
    Idle,
    Searching,
    Moving,
    Attacking,
    Parrying,
    Stunned,
    Fleeing
}