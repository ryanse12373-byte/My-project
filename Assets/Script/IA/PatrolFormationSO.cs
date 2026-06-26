using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFormation", menuName = "Formation/NewFormation")]
public class PatrolFormationSO : ScriptableObject
{
    public List<Vector3> slots;
}
