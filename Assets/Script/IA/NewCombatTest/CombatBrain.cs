using UnityEngine;

public class CombatBehaviour : MonoBehaviour
{
    private CombatMovement movement;
    private CombatAttack attack;

    private float nextThinkTime;

    private bool active;

    void Awake()
    {
        movement = GetComponent<CombatMovement>();
        attack = GetComponent<CombatAttack>();
    }

    public void StartCombat()
    {
        active = true;
    }

    public void StopCombat()
    {
        active = false;
    }

    void Update()
    {
        if (!active)
            return;

        movement.FaceTarget();

        if (Time.time < nextThinkTime)
            return;

        nextThinkTime = Time.time + Random.Range(0.25f, 0.45f);

        movement.Tick();
        attack.Tick();
    }
}