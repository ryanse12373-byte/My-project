using UnityEngine;

[CreateAssetMenu(fileName = "NewNpc", menuName = "Npc/NewNpc")]
public class NpcSO : ScriptableObject
{
    [Header("state")]
    public int attackStat = 10;
    public int defenceStat = 10;
    public float strenght = 10;
    public float endurance = 20;
    public float cooldown = 1f;
    public float range = 1.5f;

    [Header("creature")]

    public FactionSO faction;
    public string firstName, lastName;
    public RaceSO race;

}
