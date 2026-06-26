using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawningIAManager : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown factionDropdown;
    [SerializeField] private TMP_Dropdown faceDropdown;
    [SerializeField] private FactionSO[] factions;
    [SerializeField] private TMP_InputField firstNameInputField;
    [SerializeField] private TMP_InputField lastNameInputField;
    [SerializeField] private TMP_InputField atackStateInputField;
    [SerializeField] private TMP_InputField defenseStateInputField;
    [SerializeField] private TMP_InputField strengthStateInputField;
    [SerializeField] private GameObject humain;
    [SerializeField] private SpectatorManager spectator;

    [SerializeField] private CustomWeaponSO weapon;

    [SerializeField] private LayerMask ground;

    private FactionSO SelectedFaction;
    private Material SelectedFace;

    private Material[] faces;
    [SerializeField] private PatrolFormationSO formation;

    public JobPackageSO leader;

    public JobPackageSO soldier;
    void Start()
    {
        IAGenerator.Instance.leader = leader;
        IAGenerator.Instance.soldier = soldier;
        gameObject.SetActive(false);

        SetupFactions();
        SetupFaces();
    }

    void SetupFactions()
    {
        factionDropdown.ClearOptions();

        List<string> options = new List<string>();

        foreach (FactionSO faction in factions)
        {
            options.Add(faction.factionName);
        }

        factionDropdown.AddOptions(options);

        factionDropdown.onValueChanged.AddListener(OnFactionChanged);

        OnFactionChanged(0);
    }

    void SetupFaces()
    {
        faceDropdown.ClearOptions();

        faces = Resources.LoadAll<Material>("Face/Materials");

        List<string> options = new List<string>();

        foreach (Material mat in faces)
        {
            options.Add(mat.name);
        }

        faceDropdown.AddOptions(options);

        faceDropdown.onValueChanged.AddListener(OnFaceChanged);

        OnFaceChanged(0);
    }

    void OnFactionChanged(int index)
    {
        SelectedFaction = factions[index];
        Debug.Log("Faction : " + SelectedFaction.factionName);
    }

    void OnFaceChanged(int index)
    {
        SelectedFace = faces[index];
        Debug.Log("Face : " + SelectedFace.name);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V) && spectator.isSpectate)
        {
            SpawnIa();
        }
        if(Input.GetKeyDown(KeyCode.B) && spectator.isSpectate)
        {
            spawnPatrol();
        }
    }

    void SpawnIa()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f, ground))
        {

            CharacterData data = new CharacterData();
            data.faction = SelectedFaction;



            //Setup Nom Prenom
            string f = firstNameInputField.text;
            if(f == "")
            {
                data.firstName = IAGenerator.Instance.GetRandomFirstName();
            }
            else
            {
                data.firstName = f;
            }


            string l = lastNameInputField.text;
            if(l == "")
            {
                data.lastName = IAGenerator.Instance.GetRandomLastName();
            }
            else
            {
                data.lastName = l;
            }



            //Setup State
            string a = atackStateInputField.text;
            int attack;
            if(a != "" && a != "0" && int.TryParse(a, out attack))
            {
                data.attackStat = attack;
            }
            else
            {
                data.attackStat = 1;
            }


            string d = defenseStateInputField.text;
            int deffense;
            if(d != "" && d != "0" && int.TryParse(d, out deffense))
            {
                data.defenseStat = deffense;
            }
            else
            {
                data.defenseStat = 1;
            }


            string s = strengthStateInputField.text;
            float strenght;
            if(s != "" && s != "0" && float.TryParse(s, out strenght))
            {
                data.strenght = strenght;
            }
            else
            {
                data.strenght = 1;
            }

            RandomFace randomFace = humain.GetComponent<RandomFace>();
            GameObject face = randomFace.face;
            randomFace.enabled = false;
            face.GetComponent<MeshRenderer>().material = SelectedFace;
            
            //Setup id pour faire genre mais en vrai c inutil pour l'instant
            data.id = Random.Range(0, 1000);

            GameObject ia;
            IAGenerator.Instance.CreatehumainFromData(data, humain, hit.point, out ia);
            SwordBuilder.SpawnWeapon(ia, weapon, weapon.offset);
            
        }
        else
        {
            Debug.LogError("Ne peut pas spawn D'ia ici");
        }


        
    }

    void spawnPatrol()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f, ground))
        {
            IAGenerator.Instance.SpawnPatrol(humain, SelectedFaction, hit.point, weapon, formation);
        }
        else
        {
            Debug.LogError("Ne peut pas spawn De patrol ici");
        }
    }

}
