using Unity.VisualScripting;
using UnityEngine;

public class Cities : MonoBehaviour
{
    private string Name;
    public float size;
    private FactionSO faction;
    public Transform center;
    private int maxPop;

    [SerializeField] private CitiesSO data;

    // le corps de base des habitant de la ville
    [SerializeField] private GameObject body;

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
            CharacterData iaData = IAGenerator.Instance.CreateCharacter(i);
            iaData.faction = faction;
            IAGenerator.Instance.CreatehumainFromData(iaData, body,center.position,i);  
        }
    }

    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawWireSphere(center.position, size);
    }
}
