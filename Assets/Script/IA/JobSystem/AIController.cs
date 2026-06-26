using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    [SerializeField] public JobPackageSO package;

    [SerializeField] private string debugCurrentJob;

    public bool hasAction;

    public List<AIJob> jobs = new List<AIJob>();

    [SerializeField] private AIJob currentJob;

    private float nextThinkTime;


    private void Start()
    {

        foreach(var factory in package.jobs)
        {
            if(factory == null)
                continue;

            AIJob job = factory.Create(this);

            if(job != null)
                jobs.Add(job);
        }
    }

    private CombatTargeting targeting;


void Awake()
{
    targeting = GetComponent<CombatTargeting>();
}




    private void Update()
    {
            debugCurrentJob = 
                currentJob != null 
                ? currentJob.GetType().Name 
                : "None";

        
        if(Time.time >= nextThinkTime)
        {
            nextThinkTime = Time.time + 1f;


            if(targeting != null)
                targeting.UpdateTarget();


            EvaluateJobs();
        }

        currentJob?.Tick();
        hasAction = currentJob != null;
    }

    void EvaluateJobs()
    {
        AIJob bestJob = null;
        float bestScore = float.MinValue;

        foreach (var job in jobs)
        {
            if (!job.CanRun())
                continue;

            float score = job.GetPriority();

            if (score > bestScore)
            {
                bestScore = score;
                bestJob = job;
            }
        }

        
        if (bestJob != currentJob)
        {
            currentJob?.Stop();

            currentJob = bestJob;

            currentJob?.Start();
        }
    }
}
