using UnityEngine;

public class CombatStats : MonoBehaviour
{
    public HumanoidStates humanoidState;

    public float Attack => humanoidState.attackStat;
    public float Strength => humanoidState.strenght;
    public float Cooldown => humanoidState.cooldown;
    public float Range => humanoidState.range;
    public float Defence => humanoidState.defenceStat;

    public float CalculateDamage(WeaponState weapon)
    {
        float s = Strength / 100f;
        float a = Attack / 100f;
        float w = weapon.damage / 100f;

        return ((s * 0.3f) + (a * 0.4f) + (w * 0.3f)) * 100f;
    }

    public float CalculateReduction()
    {
        return
            ((Strength / 100f) * 0.4f +
            (Defence / 100f) * 0.6f) * 50f;
    }
}