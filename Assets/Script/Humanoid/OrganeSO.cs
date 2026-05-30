using UnityEngine;

[CreateAssetMenu(fileName = "NewOrgan", menuName = "Health/NewOrgan")]
public class OrganSO : ScriptableObject
{
    public string organName;
    public float size;
    public int maxHealth;
    public bool vital;
    public float efficase;
    public BodyFunction function;

    public enum BodyFunction
    {
        BloodPump,
        BloodFilter,
        Respiration,
        Digestion,
        NeuralProcessing
    }
    
}
