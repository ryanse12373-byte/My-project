using UnityEngine;

public class DataIaManager : MonoBehaviour
{
    public int id;
    public Creature creature;
    public CharacterData character;
    [SerializeField] private GameObject iaPrefab;
    [SerializeField] private Transform spawnPoint;

    public void SpawnCharacterWithId(CharacterData data)
    {
        GameObject ia = Instantiate(iaPrefab, spawnPoint.position, Quaternion.identity);
        Creature creature = ia.GetComponent<Creature>();
        HumanoidStates humanoidStates = ia.GetComponent<HumanoidStates>();
        creature.data = data;
        creature.faction = data.faction;
        creature.firstName = data.firstName;
        creature.lastName = data.lastName;
        humanoidStates.attackStat = data.attackStat;
        humanoidStates.defenceStat = data.defenseStat;
    }

}
