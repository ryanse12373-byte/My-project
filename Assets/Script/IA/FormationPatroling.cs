using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FormationPatroling : MonoBehaviour
{
    public PatrolFormationSO data;
    public NavMeshAgent leader;
    public List<NavMeshAgent> folowers = new List<NavMeshAgent>();



    public List<Vector3> formation;
    private List<CombatTargeting> vision = new List<CombatTargeting>();

    public List<FollowLeaderJob> patrolJob = new List<FollowLeaderJob>();


    void Start()
    {
        StartCoroutine(OneFrameDelay());

    }

    IEnumerator OneFrameDelay()
    {
        yield return new WaitForEndOfFrame();
        transform.position = leader.transform.position;
        transform.rotation = leader.transform.rotation;

        formation = data.slots;

        // leader en premier
        vision.Add(leader.GetComponent<CombatTargeting>());

        // followers ensuite
        for (int i = 0; i < folowers.Count; i++)
        {
            vision.Add(folowers[i].GetComponent<CombatTargeting>());
            patrolJob.Add(folowers[i].GetComponent<AIController>().jobs.Find(x => x is FollowLeaderJob) as FollowLeaderJob);

            if(patrolJob[i] == null)
            {
                Debug.LogError("Aucun patrol job pour " + folowers[i].name);
            }
        }
    }

    void Update()
    {


        int count = Mathf.Min(folowers.Count, formation.Count);

        for (int i = 0; i < count; i++)
        {
            vision[i].UpdateTarget();

            if(vision[i].HasTarget)
            {
                if(i < patrolJob.Count)
                    patrolJob[i].canFollow = false;
                continue;
            }

            Vector3 worldPos =
                leader.transform.TransformPoint(formation[i]);

            Creature creature = new Creature
            {
                data = IAGenerator.Instance.CreateCharacter(1)
            };

            if (creature.name == "Charlie" && creature.lastName == "Kirk")
            {
                creature.data.attackStat = 100;
                creature.data.defenseStat = 100;
                creature.data.strenght = 100;
            }

            if (Vector3.Distance(folowers[i].destination, worldPos) > 0.5f)
            {
                if(patrolJob[i].canFollow)
                    folowers[i].SetDestination(worldPos);
            }
        }
    }
}