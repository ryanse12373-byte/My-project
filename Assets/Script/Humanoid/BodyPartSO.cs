using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBodyPart", menuName = "Health/NewBodyPart")]
public class BodyPartSO : ScriptableObject
{
    public string partName;

    public int maxHealth;

    public bool canBleed;

    public bool vital;

    public float size;

    public List<OrganSO> organs;
}
