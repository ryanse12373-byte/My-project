using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Job Package")]
public class JobPackageSO : ScriptableObject
{
    public List<JobFactorySO> jobs;
}