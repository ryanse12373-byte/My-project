using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public FactionSO faction;
    public CharacterData data;
    public string firstName, lastName;
    public bool isSelected;
    [SerializeField] private GameObject selected;
    public Health health;
    public RaceSO race;
    public bool isPlayer;
    public Cities citie;

    public void SetSelected(bool value)
    {
        isSelected = value;
        selected.SetActive(isSelected);
    }

    void Start()
    {
        health.blood = race.maxBlood;
        GenerateBody();
    }

    void Awake()
    {
        if (health == null)
            health = GetComponent<Health>();
    }

    void GenerateBody()
    {
        health.bodyParts.Clear();
        if (race.normalBody == null) return;
        foreach (var partSO in race.normalBody)
        {
            BodyPart part = new BodyPart();
            part.data = partSO;
            part.currentHealth = partSO.maxHealth;
            part.vital = partSO.vital;

            part.organs = new List<Organ>();

            foreach (var organSO in partSO.organs)
            {
                Organ organ = new Organ();
                organ.data = organSO;
                organ.currentHealth = organSO.maxHealth;
                organ.efficase = organSO.efficase;
                organ.function = organSO.function;
                organ.vital = organSO.vital;

                part.organs.Add(organ);
            }

            health.bodyParts.Add(part);
        }
    }
}
