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
    public Inventory inventory;

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
        
        if(inventory == null)
            inventory = GetComponent<Inventory>();
    }

    void GenerateBody()
    {
        health.bodyParts.Clear();
        if (race.normalBody == null) return;
        foreach (var partSO in race.normalBody)
        {
            BodyPart part = new BodyPart
            {
                data = partSO,
                currentHealth = partSO.maxHealth,
                vital = partSO.vital,

                organs = new List<Organ>()
            };

            foreach (var organSO in partSO.organs)
            {
                Organ organ = new Organ
                {
                    data = organSO,
                    currentHealth = organSO.maxHealth,
                    efficase = organSO.efficase,
                    function = organSO.function,
                    vital = organSO.vital
                };

                part.organs.Add(organ);
            }

            health.bodyParts.Add(part);
        }
    }
}
