using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFaction", menuName = "Faction/NewFaction")]
public class FactionSO : ScriptableObject
{
    
    public int id;

    public string factionName;

    public int playerRelation;

    public List<int> allies = new List<int>();

    public List<int> enemies = new List<int>();

    public List<int> neutral = new List<int>();
}
