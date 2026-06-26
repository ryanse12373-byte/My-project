using UnityEngine;

public class SkinColorTemp : MonoBehaviour
{
    [SerializeField] private RaceSO race;
    private Creature creature;
    [SerializeField] private Material skin;
    void Start()
    {
        creature = GetComponent<Creature>();

        if(creature.race == race)
        {
            transform.GetComponent<MeshRenderer>().material = skin;
        }
    }
}
