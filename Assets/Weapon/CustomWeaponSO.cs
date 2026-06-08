using System;
using UnityEngine;

[CreateAssetMenu]
public class CustomWeaponSO : ScriptableObject
{
    public bladeSO blade;
    public guardSO guard;
    public PommelSO Pommel;
    
    public GameObject prefab;


    public String Name;
}





