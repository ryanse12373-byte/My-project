using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour
{
    public static FactionManager Instance;

    public List<FactionSO> factions = new List<FactionSO>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static FactionSO GetFactionById(int id)
    {
        return Instance.factions.Find(f => f.id == id);
    }
}