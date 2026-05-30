using TMPro;
using UnityEngine;

public class CreatureDescription : MonoBehaviour
{
    public static CreatureDescription Instance;
    public GameObject description;

    //description
    [SerializeField] private TextMeshProUGUI  firstName;
    [SerializeField] private TextMeshProUGUI  lastName;
    [SerializeField] private TextMeshProUGUI  id;
    [SerializeField] private TextMeshProUGUI  faction;


    [SerializeField] private GameObject textTemplate;
    [SerializeField] private GameObject contain;
    [SerializeField] private TextMeshProUGUI bloodText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void instantiateDescription(Creature creature)
    {
        firstName.text = "prénom : " + creature.firstName;
        lastName.text = "nom : " + creature.lastName;
        id.text = "id : " + creature.data.id;
        faction.text = "faction : " + creature.faction.factionName;
        instantiateHealth(creature);
    }

public void instantiateHealth(Creature creature)
{
    bloodText.text = "blood : "+creature.health.blood.ToString()+"/100";
    // clear ancien UI
    foreach (Transform child in contain.transform)
    {
        Destroy(child.gameObject);
    }

    foreach (BodyPart part in creature.health.bodyParts)
    {
        GameObject textObj = Instantiate(textTemplate, contain.transform);

        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();

        string result = "";

        result += "=== " + part.data.partName + " ===\n";
        result += "HP : " + part.currentHealth + "/" + part.data.maxHealth + "\n\n";

        foreach (Organ organ in part.organs)
        {
            result += "• " + organ.data.organName +
                      "   | HP : " + organ.currentHealth +
                      "   | Eff : " + organ.efficase +
                      "   | Func : " + organ.function + "\n";
        }

        text.text = result;
        text.color = Color.gray;
    }
}
    public void ShowDescription(Creature creature)
    {
        Cursor.lockState = CursorLockMode.None;
        instantiateDescription(creature);
        description.SetActive(true);
        creature.SetSelected(true);
    }
    public void HideDescription(Creature creature)
    {
        Cursor.lockState = CursorLockMode.Locked;
        description.SetActive(false);
        creature.SetSelected(false);
    }

}


