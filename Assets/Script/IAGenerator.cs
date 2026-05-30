using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAGenerator : MonoBehaviour
{
    public static IAGenerator Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [SerializeField] private int SimulatedIANumber;
    public List<CharacterData> characterData = new List<CharacterData>();


    IEnumerator simulateIA()
    {
        while (true)
        {
            RunSimulation();
            yield return new WaitForSeconds(30);          
        }


    }


    private void IaSimulated(CharacterData ia)
    {
        ia.defenseStat += Random.Range(-10 , 10);
        ia.attackStat += Random.Range(-10 , 10);
        ia.maxHp += Random.Range(-10 , 10);

        int rand = Random.Range(0,100);
        if(rand <= 30)
        {
            IaFight(ia, characterData[Random.Range(0, characterData.Count)]);
        }

    }

    private CharacterData IaFight(CharacterData a, CharacterData b)
    {
        int scoreA = a.attackStat + a.defenseStat + Random.Range(0, 10);
        int scoreB = b.attackStat + b.defenseStat + Random.Range(0, 10);
        CharacterData loser = (scoreA >= scoreB) ? b : a;
        CharacterData winner = (scoreA <= scoreB) ? b : a;
        
        if(Random.Range(0, 100) <= 50)
        {
            characterData.Remove(loser);
        }
        else
        {
            loser.rivalIds.Add(winner.id);
        }

        return (scoreA >= scoreB) ? a : b;
        
    }

    private void RunSimulation()
    {
        var snapshot = new List<CharacterData>(characterData);

        foreach (CharacterData ia in snapshot)
        {
            if (characterData.Contains(ia))
                IaSimulated(ia);
        }
    }


    public string[] prenoms =
{
    "Adèle",
    "Adrien",
    "Agathe",
    "Alain",
    "Albert",
    "Alexandre",
    "Alice",
    "Aline",
    "Amandine",
    "Amélie",
    "André",
    "Anne",
    "Antoine",
    "Arthur",
    "Audrey",
    "Aurélie",
    "Baptiste",
    "Benjamin",
    "Bernard",
    "Brigitte",
    "Camille",
    "Caroline",
    "Catherine",
    "Charles",
    "Charlotte",
    "Christophe",
    "Claire",
    "Clément",
    "Corentin",
    "Damien",
    "Daniel",
    "David",
    "Delphine",
    "Denis",
    "Diallo",
    "Dominique",
    "Édouard",
    "Élise",
    "Emma",
    "Emmanuel",
    "Éric",
    "Étienne",
    "Fabien",
    "Fabrice",
    "Florence",
    "François",
    "Gabriel",
    "Gaëlle",
    "Georges",
    "Gérard",
    "Guillaume",
    "Hélène",
    "Henri",
    "Hugo",
    "Isabelle",
    "Jacques",
    "Jean",
    "Jérôme",
    "Joël",
    "Jonathan",
    "Joseph",
    "Julie",
    "Julien",
    "Karine",
    "Kevin",
    "Laurent",
    "Léa",
    "Louis",
    "Luc",
    "Lucas",
    "Lucie",
    "Marc",
    "Margaux",
    "Marie",
    "Marion",
    "Mathieu",
    "Maxime",
    "Michel",
    "Monique",
    "Nathalie",
    "Nicolas",
    "Nina",
    "Noémie",
    "Olivier",
    "Patricia",
    "Patrick",
    "Paul",
    "Philippe",
    "Pierre",
    "Quentin",
    "Raphaël",
    "Richard",
    "Robert",
    "Romain",
    "Sabrina",
    "Samuel",
    "Sandrine",
    "Sarah",
    "Sébastien",
    "Sophie",
    "Stéphane",
    "Sylvie",
    "Thomas",
    "Valérie",
    "Victor",
    "Vincent",
    "Xavier",
    "Yann",
    "Yves"
};

    public string[] nomsDeFamille =
{
    "Adam",
    "Albert",
    "Allard",
    "Andre",
    "Arnaud",
    "Aubert",
    "Barbier",
    "Baron",
    "Benoit",
    "Berger",
    "Bernard",
    "Blanc",
    "Blanchard",
    "Bonnet",
    "Boucher",
    "Bourgeois",
    "Boyer",
    "Brun",
    "Caron",
    "Charles",
    "Chevalier",
    "Clement",
    "Colin",
    "David",
    "Denis",
    "Deschamps",
    "Dubois",
    "Dufour",
    "Dumas",
    "Dupont",
    "Durand",
    "Faure",
    "Fernandez",
    "Fontaine",
    "Fournier",
    "Francois",
    "Gaillard",
    "Garcia",
    "Garnier",
    "Gauthier",
    "Gerard",
    "Gilbert",
    "Girard",
    "Gourgue",
    "Guerin",
    "Guillot",
    "Henry",
    "Hubert",
    "Jacob",
    "Jean",
    "Joly",
    "Lacroix",
    "Lambert",
    "Laurent",
    "Leclerc",
    "Lecomte",
    "Lefebvre",
    "Legrand",
    "Lemaire",
    "Lemoine",
    "Leroy",
    "Lopez",
    "Marchand",
    "Martin",
    "Masson",
    "Mathieu",
    "Mercier",
    "Meunier",
    "Michel",
    "Moreau",
    "Morin",
    "Moulin",
    "Nicolas",
    "Nigger",
    "Noel",
    "Olivier",
    "Perrin",
    "Petit",
    "Philippe",
    "Picard",
    "Pierre",
    "Poirier",
    "Renard",
    "Renaud",
    "Rey",
    "Richard",
    "Robert",
    "Robin",
    "Roche",
    "Roger",
    "Roux",
    "Roy",
    "Schmitt",
    "Thomas",
    "Vidal",
    "Vincent",
    "Weber"
};

    void Start()
    {
        initializeIA();
        StartCoroutine(simulateIA());
    }

    private void initializeIA()
    {
        for (int i = 0; i < SimulatedIANumber; i++)
        {
            CharacterData  ia = CreateCharacter(i);
            characterData.Add(ia);
        }

    }

    public CharacterData  CreateCharacter(int id)
    {
        CharacterData ia = new CharacterData
        {
            id = id,
            firstName = prenoms[Random.Range(0 , prenoms.Length)],
            lastName = nomsDeFamille[Random.Range(0 , nomsDeFamille.Length)],
            defenseStat = Random.Range(2, 20),
            attackStat = Random.Range(2, 20),
            maxHp = Random.Range(80, 150),
            weaponType = (weaponType)Random.Range(0, System.Enum.GetValues(typeof(weaponType)).Length)
        };

        return ia;

    }
}

[System.Serializable]
public class CharacterData 
{
    public int id;
    public FactionSO faction;
    public int defenseStat;
    public int attackStat;
    public int maxHp;
    public string lastName;
    public string firstName;
    public weaponType weaponType;
    public List<int> rivalIds = new List<int>();

}

public enum weaponType
{
    range,
    melee
}



