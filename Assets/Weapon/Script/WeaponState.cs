using UnityEngine;

public class WeaponState : MonoBehaviour
{
    public int damage;
    public int attackBonus;
    public int defenseBonus;
    public DamageType damageType;
    public Rarity rarity;


    public enum DamageType
    {
        blunt,
        sharp
    }

    public enum Rarity
    {
        normale,
        rare
    }
}
