using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRace", menuName = "Race/NewRace")]
public class RaceSO : ScriptableObject
{
    public string raceName;
    public float maxBlood;

    public List<BodyPartSO> normalBody;
    public List<OrganSO.BodyFunction> requiredFunctions;
    
}