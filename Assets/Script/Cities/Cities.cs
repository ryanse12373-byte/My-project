using UnityEngine;
using UnityEngine.AI;

public class Cities : MonoBehaviour
{
    private string Name;
    public float size;
    private FactionSO faction;
    public Transform center;
    private int maxPop;
    [SerializeField] private JobPackageSO[] citiesJobs;

    [SerializeField] private CitiesSO data;

    // le corps de base des habitant de la ville
    [SerializeField] private GameObject body;
    [SerializeField] private CustomWeaponSO weaponData;
    public GeneriqueWorkStation[] workStations;
    public BedWorkStation[] bedWorkStations;

    [SerializeField] private Terrain terrain;

    void Start()
    {
        Name = data.Name;
        faction = data.faction;
        maxPop = data.maxPop;
        PopulateCities();
    }

    void PopulateCities()
    {
        if(center == null)
        {
            Debug.LogError("pas de centre assigner a la ville <b>" + Name +"</b>");
            return;
        }
        for (int i = 0; i < maxPop; i++)
        {
            GameObject ia;
            CharacterData iaData = IAGenerator.Instance.CreateCharacter(i);
            iaData.faction = faction;
            IAGenerator.Instance.CreatehumainFromData(iaData, body,center.position, out ia ,i);
            IAGenerator.Instance.AddJobsToIA(ia, GetRandomJobPackage(citiesJobs));
            if (ia.GetComponent<AIController>().package.name != "Worker(Bottom)")
            {
                SwordBuilder.SpawnWeapon(ia,weaponData, weaponData.offset);
            }

            ia.GetComponent<Creature>().citie = this;

        }
    }

    private JobPackageSO GetRandomJobPackage(JobPackageSO[] jobs)
    {
        JobPackageSO job_ = jobs[Random.Range(0, jobs.Length)];
        return job_;
    }


    public Vector3 GetRandomPointInCitie()
    {
        Vector3 randomCirclePos = (Random.insideUnitSphere * size)+ center.transform.position ;
        Vector3 point = new Vector3(randomCirclePos.x, terrain.SampleHeight(randomCirclePos), randomCirclePos.z);

        NavMeshHit hit;

        if (NavMesh.SamplePosition(point, out hit, 5f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        Debug.LogError("Essaye de trouver un point au hasard dans la ville mais n'a pas reussi");
        return point;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawWireSphere(center.position, size);
    }

    void Awake()
    {
        SetupWorkStation();
    }

    void SetupWorkStation()
    {
        workStations = GetComponentsInChildren<GeneriqueWorkStation>();
        bedWorkStations = GetComponentsInChildren<BedWorkStation>();
    }
}
