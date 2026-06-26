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
    public WorkStation[] workStations;

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
            SwordBuilder.SpawnWeapon(ia,weaponData, weaponData.offset);
            IAGenerator.Instance.AddJobsToIA(ia, GetRandomJobPackage(citiesJobs));
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
        Debug.LogError("epstein4ever");
        return point;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Instantiate(workStations[0], GetRandomPointInCitie(), Quaternion.identity);
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawWireSphere(center.position, size);
    }
}







