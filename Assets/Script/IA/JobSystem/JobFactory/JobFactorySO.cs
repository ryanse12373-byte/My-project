using UnityEngine;

public abstract class JobFactorySO : ScriptableObject
{
    public abstract AIJob Create(AIController controller);
    
}