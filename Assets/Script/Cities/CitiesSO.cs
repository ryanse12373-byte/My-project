using UnityEngine;

[CreateAssetMenu(fileName = "NewCity", menuName = "City")]
public class CitiesSO : ScriptableObject
{
    public string Name;
    public FactionSO faction;
    public int maxPop;
}
